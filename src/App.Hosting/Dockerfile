#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 81

FROM base AS final
RUN mkdir -p /data

WORKDIR /app
COPY  . /data
COPY start.sh /data

RUN chmod +x /data/start.sh
ENTRYPOINT ["/data/start.sh"]
