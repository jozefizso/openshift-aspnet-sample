FROM microsoft/dotnet:2.2-sdk-alpine AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine AS runtime
LABEL io.k8s.description="Sample application for running on OpenShift platform." \
      io.k8s.display-name="ASP.NET Core sample app for OpenShift" \
      io.openshift.expose-services="8080:http"

EXPOSE 8080
ENV HOME=/app \
    ASPNETCORE_URLS=http://*:8080

COPY ./usr/bin /usr/bin

WORKDIR /app
COPY --from=build-env /app/out ./

RUN adduser -u 1001 -S -h ${HOME} -s /sbin/nologin default
RUN chown -R 1001:0 /app && fix-permissions /app

ENTRYPOINT ["dotnet", "openshift-aspnet-sample.dll"]
USER 1001