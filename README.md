# Notiflow
Using firebase push notification dotnet core with rabbitmq and rest services

Notiflow is designed to make to send Push Notification using Google's Firebase Cloud Messaging

# Implemented for:

- .Net Core 
- RabbitMq
- Masstransit


### Installation

Install rabbit mq  server for docker

```sh
docker run -d --hostname my-rabbit --name myrabbit -e RABBITMQ_DEFAULT_USER=admin -e RABBITMQ_DEFAULT_PASS=123456 -p 5672:5672 -p 15672:15672 rabbitmq:3-management

```
