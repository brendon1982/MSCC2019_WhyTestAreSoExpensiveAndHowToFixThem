cd /c/systems/Talks/Why\ tests\ are\ so\ expensive\ and\ how\ to\ fix\ them/
currentTag=$(git tag -l --points-at HEAD)
git checkout tags/$(($currentTag - 1))
