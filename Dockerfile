FROM microsoft/dotnet:2.1-sdk AS build-env

ENV NODE_VERSION 8.9.4
ENV NODE_DOWNLOAD_SHA 21fb4690e349f82d708ae766def01d7fec1b085ce1f5ab30d9bda8ee126ca8fc
RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs

ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS http://*:5000

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -o /app

#RUN cp ~/.microsoft/usersecrets/4C3AE4AE-33CD-4C9B-8546-00C65C782A08/secrets.json /app
#RUN mkdir -p ~/.microsoft/usersecrets/4C3AE4AE-33CD-4C9B-8546-00C65C782A08
#WORKDIR /app
#COPY --from=build-env /app/out .
#COPY --from=build-env /app/secrets.json ~/.microsoft/usersecrets/4C3AE4AE-33CD-4C9B-8546-00C65C782A08/

EXPOSE 5001
EXPOSE 5000

RUN dotnet user-secrets set ConnectionStrings:ChatStorage "DefaultEndpointsProtocol=https;AccountName=nextgamestest;AccountKey=nBU2UB59JteQOMvXJVAcvx1mGmkNFlDX5aF9pSM4+7noj4e+cdHGKQadFyit2XfvIrmUwltw+xzDgDvjVWG6zQ==;EndpointSuffix=core.windows.net"
RUN dotnet user-secrets set Azure:SignalR:ConnectionString "Endpoint=https://ngtest.service.signalr.net;AccessKey=gXHk1stcDV77Tnzx43L1st6j042MTpSlcHEXQsp2hKg=;"

CMD ["dotnet", "NGTest.dll"]