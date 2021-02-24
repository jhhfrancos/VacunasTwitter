#!/bin/bash

#while true
#do
cd ./twitter4j
cat ./terms.txt | grep -v '^#' | xargs -I{} -n1 -P1 sh -c "java -jar target/twitdemo.jar -q '{}' -m 10 -w 50000 >> list.txt 2>> listAll.txt ; sort -u -o list.txt list.txt ; sort -u -o listAll.txt listAll.txt"
bash ./auto-timeline.sh
#echo "hola" >> texto.txt
#done


