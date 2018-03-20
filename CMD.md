## Commands

## Install

```
brew install pandoc
brew cask install mactex
```

## Build

```bash
rm /Users/wk/.dotnet/tools/wk-fake-letter
cake -target=Pack
dotnet install tool -g wk.FakeLetter --source ./publish

wk-fake-letter template/Letter.md 10
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