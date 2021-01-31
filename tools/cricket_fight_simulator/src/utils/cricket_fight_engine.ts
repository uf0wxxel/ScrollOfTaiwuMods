import * as _ from 'lodash';
import { ICricketBattleData, ICricketData } from './cricket_data_utils';

export interface ICricketFlightLogRecord {
    message: string;
    roundChanged?: boolean;
    round?: number;
}

function roll(rate: number): boolean {
    return rate > 0 && Math.random() < rate;
}

export function sleepAsync(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
}

export function isCricketLost(c: ICricketBattleData): boolean {
    return c.耐力 <= 0 || c.斗性 <= 0 || c.durability <= 0;
}

function checkWinner(
    a: ICricketData,
    b: ICricketData,
    log: (r: ICricketFlightLogRecord) => void,
    display: (c: ICricketData) => string): boolean {
    if (isCricketLost(a)) {
        const isDead = a.durability <= 0;
        log({
            message: `${display(a)}已${isDead ? '死亡' : '失去战斗力'}，${display(b)}获胜。`
        });
        return true;
    } else if (isCricketLost(b)) {
        const isDead = b.durability <= 0;
        log({
            message: `${display(b)}已${isDead ? '死亡' : '失去战斗力'}，${display(a)}获胜。`
        });
        return true;
    }

    return false;
}

function substract(n: number, by: number, minValue = 0): number {
    const result = n - by;
    return Math.max(minValue, result);
}

function substractCricketData(
    c: ICricketData,
    by: { hp?: number, sp?: number, durability?: number },
    log: (r: ICricketFlightLogRecord) => void,
    display: (c: ICricketData) => string) {
    let logSuffix = '';
    if (by?.hp ?? 0 > 0) {
        c.耐力 = substract(c.耐力, by.hp!);
        logSuffix += `耐力${c.耐力}(-${by.hp!})，`;
    }

    if (by?.sp ?? 0 > 0) {
        c.斗性 = substract(c.斗性, by.sp!);
        logSuffix += `斗性${c.斗性}(-${by.sp!})，`;
    }

    if (by?.durability ?? 0 > 0) {
        c.durability = substract(c.durability, by.durability!);
        logSuffix += `耐久${c.durability}(-${by.durability!})，`;
    }

    if (logSuffix) {
        log({ message: `${display(c)}受到攻击，${_.trim(logSuffix, '，')}。` });
    }
}

function addInjury(
    c: ICricketData,
    log: (r: ICricketFlightLogRecord) => void,
    display: (c: ICricketData) => string) {
    if (!c.InjuryLevel) {
        c.InjuryLevel = {
            耐力: 0,
            斗性: 0,
            气势: 0,
            角力: 0,
            牙钳: 0,
        };
    }

    let injureItem = '';
    let valueChanged = -1;
    if (roll(0.65)) {
        if (roll(0.5)) {
            c.InjuryLevel.耐力 += 1;
            injureItem = '耐力';
            valueChanged *= 5;
        }
        else {
            c.InjuryLevel.斗性 += 1;
            injureItem = '斗性';
            valueChanged *= 5;
        }
    }
    else {
        switch (_.random(0, 2, false)) {
            case 0:
                c.InjuryLevel.气势 += 1;
                injureItem = '气势';
                break;
            case 1:
                c.InjuryLevel.角力 += 1;
                injureItem = '角力';
                break;
            default:
                c.InjuryLevel.牙钳 += 1;
                injureItem = '牙钳';
                break;
        }
    }

    if (injureItem) {
        log({ message: `${display(c)}被重伤，${injureItem}上限${valueChanged}` });
    }
}

function getSingleAttackDamageWinner(
    attacker: ICricketData,
    defender: ICricketData,
    isHomeCounterAttack: boolean,
    isAwayCounterAttack: boolean,
    log: (r: ICricketFlightLogRecord) => void,
    display: (c: ICricketData) => string): boolean {
    const isCounterAttack = isHomeCounterAttack || isAwayCounterAttack;

    const battleTypeTextRaw = isCounterAttack ? '反击' : '主动进攻';
    const battleTypeText = `【${battleTypeTextRaw}】 `;
    const hpDamageWeapon = isHomeCounterAttack ? '角力' : '牙钳';

    const possibleSpDamage = substract(attacker.气势, (attacker.InjuryLevel?.气势 ?? 0), Math.floor(attacker.气势 * 3 / 10));
    const possiblePlierDamage = substract(attacker.牙钳, (attacker.InjuryLevel?.牙钳 ?? 0), Math.floor(attacker.牙钳 * 3 / 10));
    const possibleWrestlingDamage = substract(attacker.角力, (attacker.InjuryLevel?.角力 ?? 0), Math.floor(attacker.角力 * 3 / 10));
    let hpDamage = isHomeCounterAttack ? possibleWrestlingDamage : possiblePlierDamage;

    let spDamage = isCounterAttack ? possibleSpDamage : 0;
    let critical = false;
    let hurt = false;

    log({ message: `${battleTypeText}${display(attacker)}发动${battleTypeTextRaw}，当前${hpDamageWeapon}伤害${hpDamage}，气势伤害${spDamage}。` });

    critical = roll(attacker.暴击率);
    if (critical) {
        hpDamage += attacker.暴击增伤;
        const spDamageIncrease = possibleSpDamage - spDamage;
        spDamage = possibleSpDamage;
        log({ message: `【暴击】 ${display(attacker)}触发暴击(${(attacker.暴击率 * 100).toFixed(0)}%)，当前${hpDamageWeapon}伤害${hpDamage}(+${attacker.暴击增伤})，气势伤害${spDamage}(+${spDamageIncrease})。` });
    }

    const blocked = roll(defender.格挡概率);
    const hurtRate = attacker.暴击率 + attacker.击伤概率修正;
    if (blocked) {
        hpDamage = substract(hpDamage, defender.格挡值);
        spDamage = substract(spDamage, defender.格挡值);
        log({ message: `【格挡】 ${display(defender)}触发格挡(${(defender.格挡概率 * 100).toFixed(0)}%)。 对方当前${hpDamageWeapon}伤害${hpDamage}(-${defender.格挡值})，气势伤害${spDamage}(-${defender.格挡值})。` });
    }
    else if (critical) {
        hurt = roll(hurtRate);
    }

    if (hpDamage > 0 || spDamage > 0 || hurt) {
        substractCricketData(defender, { hp: hpDamage, sp: spDamage, durability: hurt ? 1 : 0 }, log, display);
        if (hurt) {
            if (roll(hurtRate)) {
                addInjury(defender, log, display);
            }
        }

        if (checkWinner(attacker, defender, log, display)) {
            return true;
        }
    }

    return false;
}

function checkRoundWinner(
    attacker: ICricketData,
    defender: ICricketData,
    // round: number,
    // counterAttackTriggeredInRound: number,
    log: (r: ICricketFlightLogRecord) => void,
    display: (c: ICricketData) => string): boolean {
    // log({ message: `【第${round}轮】 开始。`, roundChanged: true, round: round, });
    if (getSingleAttackDamageWinner(attacker, defender, false, false, log, display)) {
        return true;
    }

    const caRateDropBy = 0.05;
    let counterAttackTriggeredInRound = 0;

    let caAttacker = defender;
    let caDefender = attacker;
    let counterAttack = roll(caAttacker.反击率 - caRateDropBy * counterAttackTriggeredInRound);

    while (counterAttack) {
        counterAttackTriggeredInRound += 1;

        const isHomeCounterAttack = caAttacker === defender;
        log({ message: `${display(caAttacker)}触发${isHomeCounterAttack ? '己' : '对'}方半场反击(${((caAttacker.反击率 - 0.05 * (counterAttackTriggeredInRound - 1)) * 100).toFixed(0)}%)。` });
        if (getSingleAttackDamageWinner(caAttacker, caDefender, isHomeCounterAttack, !isHomeCounterAttack, log, display)) {
            return true;
        }

        const tmp = caAttacker;
        caAttacker = caDefender;
        caDefender = tmp;
        counterAttack = roll(caAttacker.反击率 - caRateDropBy * counterAttackTriggeredInRound);
    }

    return false;
}

export function cricketFight(
    a: ICricketData,
    b: ICricketData,
    log: (r: ICricketFlightLogRecord) => void = () => { },
    display: (c: ICricketData) => string = c => `${c.side}${c.name}`): ICricketData {
    if (checkWinner(a, b, log, display)) {
        return isCricketLost(a) ? b : a;
    }

    // Assume a attacks first
    let attacker = a;
    let defender = b;

    let round = 0;
    let roundLimit = 1000;
    do {
        round += 1;
        let swapAttacker: boolean;
        if (a.气势 > b.气势) {
            // 20% chance to let b attack first
            swapAttacker = roll(0.2);
            b.斗性 = substract(b.斗性, a.气势);
            log({
                message: `【第${round}轮】 ${display(a)}发动气势攻击(${a.气势})，${display(b)}斗性${b.斗性}(-${a.气势})`
            });
        }
        else if (a.气势 < b.气势) {
            // 80% chance to let b attack first
            swapAttacker = roll(0.8);
            a.斗性 = substract(a.斗性, b.气势);
            log({
                message: `【第${round}轮】 ${display(b)}发动气势攻击(${b.气势})，${display(a)}斗性${a.斗性}(-${b.气势})`
            });
        } else {
            // 50% chance to let b attack first
            swapAttacker = roll(0.5);
            log({
                message: `【第${round}轮】 双方气势旗鼓相当`
            });
        }

        if (checkWinner(a, b, log, display)) {
            return isCricketLost(a) ? b : a;
        }

        if (swapAttacker) {
            const tmp = attacker;
            attacker = defender;
            defender = tmp;
        }
    } while (!checkRoundWinner(attacker, defender, log, display)
    && !checkRoundWinner(defender, attacker, log, display) && round < roundLimit)

    round = 1;
    roundLimit = 1000;
    // while (!checkRoundWinner(attacker, defender, round++, false, 0, log, display)
    //     && round < roundLimit) {
    //     var tmp = attacker;
    //     attacker = defender;
    //     defender = tmp;
    // }

    return isCricketLost(a) ? b : a;
}
