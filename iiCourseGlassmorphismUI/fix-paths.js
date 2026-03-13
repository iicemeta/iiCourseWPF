import fs from 'fs';
import path from 'path';

const distPath = './dist/index.html';

if (fs.existsSync(distPath)) {
  let content = fs.readFileSync(distPath, 'utf-8');

  // 修复 CSS 路径
  content = content.replace(/href="\/\.\//g, 'href="./');
  content = content.replace(/href="\/_astro\//g, 'href="./_astro/');

  // 修复 JS 路径 - 处理各种变体
  content = content.replace(/src="\/\.\/\.\/\.\//g, 'src="./');
  content = content.replace(/src="\/\.\//g, 'src="./');
  content = content.replace(/src="\/_astro\//g, 'src="./_astro/');

  // 清理多余斜杠 - 处理 ././ 变体
  content = content.replace(/\.\/\.\//g, './');
  content = content.replace(/\.\/\//g, './');
  content = content.replace(/\.\.\//g, './');
  content = content.replace(/src="\.\.\//g, 'src="./');
  content = content.replace(/src="\.\/\.\//g, 'src="./');

  // 为 WebView2 添加一个标记，方便 C# 代码识别并处理路径
  // 在 <head> 中添加 base 标签，让相对路径基于当前文档
  if (!content.includes('<base href=')) {
    content = content.replace('<head>', '<head>\n    <base href="./">');
  }

  fs.writeFileSync(distPath, content);
  console.log('Paths fixed successfully!');
} else {
  console.error('index.html not found!');
}
