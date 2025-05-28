# Media Search ASP.Net Core Web Api

See the working code at https://groover.tech/

The backend service for the Movie Search web app is a simple
.Net Core Web Api.


The "about" page has more info on the project:
[About Page](https://groover.tech/about)

It's important to set user secrets in your secrets.json for the TMDB rest api key, Auth0, and Cosmos DB

```json
  "Auth0": {
    "Domain": "",
    "Audience": "",
    "ClientId": "",
    "ClientSecret": ""
  },
  "CosmosDb": {
    "EndpointUri": "",
    "PrimaryKey": ""
  },
  "MovieApiKey": ""
```
You will need to sign up for an API key to run the backend 
service

[TMDB Developer FAQ](https://developer.themoviedb.org/docs/faq)

# Misc
Interesting finding during testing, the movie Titanic (1997) 
made more money than an Int32 can hold: JSON integer 
2,264,162,353 is too large for an Int32. Avengers Endgame also had the same issue

The default generated view model was an Int32 so I had to manually
change it to an Int64 after researching the exceptions I was getting
trying to retrieve data from the TMDB api.
