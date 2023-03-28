#!/bin/bash
# Number of attempts
WAIT=10
# To keep count of how many attempts have passed
count=0

docker-compose up -d

while ! docker exec src_postgres_1 pg_isready -q  > /dev/null 2> /dev/null; do
    sleep 2

    if [ $count -gt $WAIT ]; then
        docker-compose down
        docker-compose up &
        count=0
    fi
    ((count++))
done

echo Postgres is ready

redisCount=0
while ! docker exec src_redis_1 redis-cli ping > /dev/null 2> /dev/null; do
    sleep 2

    if [ $redisCount -gt $WAIT ]; then
        docker-compose down
        docker-compose up &
        redisCount=0
    fi
    ((redisCount++))
done

echo Redis is ready