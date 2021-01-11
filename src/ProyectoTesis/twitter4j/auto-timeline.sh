#!/bin/bash

cat list.txt | xargs -I{} -n1 -P1 sh -c 'OFF=`tail -n1 data/{} | cut -f1 -d" "`;OFF=${OFF:-0};java -jar target/twitdemo.jar -w 10000 -u {} -o $OFF >> data/{};'
