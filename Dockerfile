# תמונת בסיס של .NET SDK לבניית היישום
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# יצירת תיקיית עבודה עבור הבנייה
WORKDIR /src

# העתקת קובץ MonthlyDataApi.csproj לתוך התמונה
COPY . .

# הרצת פקודת restore
RUN dotnet restore "MonthlyDataApi/MonthlyDataApi.csproj"

# בניית היישום
RUN dotnet build "MonthlyDataApi/MonthlyDataApi.csproj" -c Release -o /app/build

# פרסום היישום
RUN dotnet publish "MonthlyDataApi/MonthlyDataApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# תמונת הבסיס של ASP.NET כדי להריץ את היישום
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# העתקת קבצי הבנייה לפלט
COPY --from=build /app/publish .

# הרצת היישום
ENTRYPOINT ["dotnet", "MonthlyDataApi.dll"]
