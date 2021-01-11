#!/bin/bash

while true
do
  cat terms.txt | grep -v '^#' | xargs -I{} -n1 -P1 sh -c "java -jar target/twitdemo.jar -q '{}' -m 1000 -w 50000 >> list.txt 2>> listAll.txt ; sort -u -o list.txt list.txt ; sort -u -o listAll.txt listAll.txt"
  ./auto-timeline.sh

done
