FROM ://microsoft.com As build
WORKDIR /src

COPY ["Al_Shami_AISS_.csproj", "./"]
RUN dotnet restore "Al_Shami_AISS_.csproj"

COPY . .
RUN dotnet publish "Al_Shami_AISS_.csproj" -c Release -o /app/publish

FROM ://microsoft.com AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Al_Shami_AISS_.dll"]
