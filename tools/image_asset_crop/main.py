import os
import sys
import re
import codecs
from PIL import Image, ImageDraw


class SubImageInfo(object):
    def __init__(self, file_name):
        self.file_name = file_name
        self.x = 0
        self.y = 0
        self.width = 0
        self.height = 0

    def __str__(self):
        return f'{self.file_name}|{self.x}|{self.y}|{self.width}|{self.height}'


def extract_sub_images_info(meta_file_path: str) -> [SubImageInfo]:
    with open(meta_file_path) as fp:
        lines = fp.readlines()
    results: [SubImageInfo] = []
    current: SubImageInfo = None
    for line in lines:
        match = re.match(r'^\s*name:\s+(?P<file>.+)\s*$', line)
        if match:
            sub_file_name = match.group('file')
            current = SubImageInfo(sub_file_name)
            results.append(current)
            continue
        for var in ('x', 'y', 'width', 'height'):
            var_match = re.match(f'^\\s*{var}:\\s+(?P<{var}>\d+)\\s*$', line)
            if var_match:
                value = int(var_match.group(var))
                setattr(current, var, value)
    return results


if __name__ == '__main__':
    css_content = ''
    for root, dirs, files in os.walk("./input"):
        for file_name in files:
            if file_name[-5:] == '.meta':
                meta_file_path = os.path.join(root, file_name)
                image_file_name = file_name.rstrip('.meta')
                image_file_path = meta_file_path.rstrip('.meta')
                print(meta_file_path)
                print(image_file_path)
                info_list = extract_sub_images_info(meta_file_path)

                max_x = max(info_list, key=lambda i: i.x).x + \
                    info_list[0].width
                min_y = min(info_list, key=lambda i: i.y).y

                with Image.open(image_file_path) as img:
                    new_width = max_x
                    new_height = img.height - min_y
                    print(new_width, new_height)
                    sub_img = img.crop(
                        (0, 0, new_width, new_height))
                    sub_img = sub_img.resize(
                        (new_width // 2, new_height // 2), Image.ANTIALIAS)
                    output_file_name_png = os.path.join(
                        'output', image_file_name)
                    output_file_name_webp = output_file_name_png.replace(
                        '.png', '.webp')
                    # sub_img.save(output_file_name_png, quality=100)
                    # os.system(
                    #     f'cwebp {output_file_name_png} -o {output_file_name_webp}')

                    for info in info_list:
                        width = info.width
                        height = info.height
                        x = info.x
                        y = img.height - info.y - height

                        x = x // 2
                        y = y // 2

                        css_content += f"\n.{info.file_name} {{background: url(./assets/images/crickets/{image_file_name.replace('.png', '.webp')}) -{x}px -{y}px;}}"

                # border_pixel_to_cut = 0
                # with Image.open(image_file_path) as img:
                #     for info in info_list:
                #         sub_img = img.crop(
                #             (info.x + border_pixel_to_cut, img.width - info.y - info.height + border_pixel_to_cut, info.x + info.width - border_pixel_to_cut, img.width - info.y - border_pixel_to_cut))
                #         if '_36_' in info.file_name:
                #             sub_img = sub_img.resize(
                #                 (200, 200), Image.ANTIALIAS)
                #         else:
                #             sub_img = sub_img.resize(
                #                 (100, 100), Image.ANTIALIAS)

                #         output_file_name_no_ext = os.path.join(
                #             'output', f'{info.file_name}')
                #         output_file_name_png = f'{output_file_name_no_ext}.png'
                #         output_file_name_webp = f'{output_file_name_no_ext}.webp'
                #         sub_img.save(output_file_name_png, quality=100)
                #         # sub_img.save(output_file_name_webp, format="WebP")
                #         os.system(
                #             f'cwebp {output_file_name_png} -o {output_file_name_webp}')

    with codecs.open(os.path.join('output', 'crickets.scss'), 'w', 'utf-8-sig') as fp:
        fp.write(css_content)
