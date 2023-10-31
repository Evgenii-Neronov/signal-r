#собрать имейдж
docker build -t hub .

#запустить 3 экземпляра
docker run -d -p 7001:80 hub
docker run -d -p 7002:80 hub
docker run -d -p 7003:80 hub