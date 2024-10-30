#!/bin/bash
#export GET_TVSHOW_TOTAL_LENGTH_BIN=/home/user/GetTvShowTotalLength/bin/Debug/net8.0/GetTvShowTotalLength

if [ -z $GET_TVSHOW_TOTAL_LENGTH_BIN ] 
then
  echo "App env variable is not set" >&2
fi

declare -a pids
declare -a shows

while read arg; do
  $GET_TVSHOW_TOTAL_LENGTH_BIN "$arg" >> results.txt & pids+=("$!")
  shows+=("$arg")
done

exit_status=10
iterator=0
for pid in "${pids[@]}" 
do
  wait "$pid";
  if (( exit_status == $? )) then
    echo "Could not get info for ${shows[$iterator]}" >&2
  fi
  iterator=$iterator+1
done

wait

shortestMin=9999999
shortestName=''
longestMin=0
longestName=''


while read arg; do
 minutes=$(echo "$arg" | cut -d';' -f2)
 if(($shortestMin > $minutes)) then
  shortestMin=$minutes
  shortestName=$(echo "$arg" | cut -d';' -f1)
fi
 if(($longestMin < $minutes)) then
  longestMin=$minutes
  longestName=$(echo "$arg" | cut -d';' -f1)
fi

done < results.txt;

echo "The shortest show: $shortestName ($(($shortestMin / 60))h $(($shortestMin % 60))min)"
echo "The longest show: $longestName ($(($longestMin / 60))h $(($longestMin % 60))min)"

rm results.txt