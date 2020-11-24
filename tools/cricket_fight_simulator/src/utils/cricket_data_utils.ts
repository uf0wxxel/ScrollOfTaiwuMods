import cricketPartDataRaw from './../assets/cricketDate.json';
import * as _ from 'lodash';

import cricketImages from './../assets/images/crickets/*.webp';

export interface ICricketBattleData {
    durability: number,

    耐力: number,
    斗性: number,

    气势: number,
    角力: number,
    牙钳: number,

    InjuryLevel?: {
        耐力: number,
        斗性: number,
        气势: number,
        角力: number,
        牙钳: number,
    },

    暴击率: number,
    暴击增伤: number,
    击伤概率修正: number,
    格挡概率: number,

    格挡值: number,
    反击率: number,
}

export interface ICricketData extends ICricketBattleData {
    level: number, // 1-9
    name: string,
    desc?: string,
    imageUrl?: string,
    side?: string,
}

export interface ICricketPartData extends ICricketData {
    id: number,
    isColorPart: boolean,
    isBodyPart: boolean,
    partName: string,
    partNameAsSuffix?: string,

    namingPriority: number, // 1-9
    imageReferenceId: number,
}

function parseCricketPart(id: number, dict: { string: string }): ICricketPartData {
    const level = parseInt(dict["1"]);
    // 呆物 id 为 0
    const isColorPart = id > 0 && parseInt(dict["3"]) != 0 && level < 8;
    const isBodyPart = id > 0 && !isColorPart && level < 8;

    const nameArray = (dict["0"] as string).split('|');

    return {
        id: id,
        level: level,
        desc: dict["99"],
        imageUrl: '',
        name: '',

        isColorPart: isColorPart,
        isBodyPart: isBodyPart,
        partName: nameArray[0],
        partNameAsSuffix: nameArray.length > 1 ? nameArray[1] : '',

        namingPriority: parseInt(dict["2"]), // 1-9
        imageReferenceId: parseInt(dict["96"]),

        durability: 20,
        耐力: parseInt(dict["11"]),
        斗性: parseInt(dict["12"]),

        气势: parseInt(dict["21"]),
        角力: parseInt(dict["22"]),
        牙钳: parseInt(dict["23"]),

        暴击率: parseFloat(dict["31"]) / 100,
        暴击增伤: parseInt(dict["32"]),
        击伤概率修正: parseFloat(dict["36"]) / 100,
        格挡概率: parseFloat(dict["33"]) / 100,
        格挡值: parseInt(dict["34"]),
        反击率: parseFloat(dict["35"]) / 100,
    };
}

function parseCricketParts(): ICricketPartData[] {
    var results: ICricketPartData[] = [];
    const cricketPartDataRawTyped = cricketPartDataRaw as unknown as { string: { string: string } };
    for (const id in cricketPartDataRawTyped) {
        results.push(parseCricketPart(parseInt(id), cricketPartDataRawTyped[id]));
    }

    return results;
}

function generateCricketCollection(): ICricketData[] {
    var kings: ICricketData[] = _.chain(cricketParts)
        .filter(p => !p.isColorPart && !p.isBodyPart)
        .map(function (p) {
            return {
                name: p.partName,
                level: p.level, // 1-9
                desc: p.desc,
                // imageUrl: cricketImages[`Cricket_36_${p.imageReferenceId}`],
                imageUrl: `Cricket_36_${p.imageReferenceId}`,
                durability: 20,
                耐力: p.耐力,
                斗性: p.斗性,
                气势: p.气势,
                角力: p.角力,
                牙钳: p.牙钳,
                暴击率: p.暴击率,
                暴击增伤: p.暴击增伤,
                击伤概率修正: p.击伤概率修正,
                格挡概率: p.格挡概率,
                格挡值: p.格挡值,
                反击率: p.反击率,
            }
        })
        .value();

    var colorParts = _.chain(cricketParts).filter(p => p.isColorPart).orderBy(p => p.level, 'desc').value();
    var bodyParts = _.chain(cricketParts).filter(p => p.isBodyPart).orderBy(p => p.level, 'desc').value();

    const results = kings;
    for (var i = 0; i < colorParts.length; i++) {
        for (var j = 0; j < bodyParts.length; j++) {
            const colorPart = colorParts[i];
            const bodyPart = bodyParts[j];
            // if (colorPart.level > bodyPart.level) {
            //     continue;
            // }

            const name = colorPart.namingPriority < bodyPart.namingPriority ? `${bodyPart.partName}${colorPart.partNameAsSuffix}` : `${colorPart.partName}${bodyPart.partName}`;
            const level = Math.max(bodyPart.level, colorPart.level);

            results.push({
                name: name,
                level: level, // 1-9
                desc: bodyPart.desc,
                // imageUrl: cricketImages[`Cricket_${colorPart.imageReferenceId}_${bodyPart.imageReferenceId}`],
                imageUrl: `Cricket_${colorPart.imageReferenceId}_${bodyPart.imageReferenceId}`,
                durability: 20,
                耐力: colorPart.耐力 + bodyPart.耐力,
                斗性: colorPart.斗性 + bodyPart.斗性,
                气势: colorPart.气势 + bodyPart.气势,
                角力: colorPart.角力 + bodyPart.角力,
                牙钳: colorPart.牙钳 + bodyPart.牙钳,
                暴击率: colorPart.暴击率 + bodyPart.暴击率,
                暴击增伤: colorPart.暴击增伤 + bodyPart.暴击增伤,
                击伤概率修正: colorPart.击伤概率修正 + bodyPart.击伤概率修正,
                格挡概率: colorPart.格挡概率 + bodyPart.格挡概率,
                格挡值: colorPart.格挡值 + bodyPart.格挡值,
                反击率: colorPart.反击率 + bodyPart.反击率,
            });
        }
    }

    return results;
}

export const cricketParts: ICricketPartData[] = parseCricketParts();
export const cricketCollection: ICricketData[] = generateCricketCollection();
