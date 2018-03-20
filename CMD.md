## Commands

## Install

```
brew install pandoc
brew cask install mactex
```

## Build

```bash
rm /Users/wk/.dotnet/tools/wk-avatar
cake -target=Pack
dotnet install tool -g wk.Avatar --source ./publish
```

## Test

```
ls /Library/TeX/texbin
pandoc CMD.md -o output-CMD.html
pandoc CMD.md --pdf-engine=xelatex -o output-CMD.pdf
pandoc CMD.md -o output-CMD.pdf --pdf-engine pdflatex
pandoc CMD.md --to=pdf -t latex -o myfile.pdf --latex-engine=/Library/TeX/texbin/pdflatex
pandoc CMD.md -o pdf-CMD.pdf --pdf-engine=pdflatex
```