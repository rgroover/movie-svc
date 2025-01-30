# Media Search ASP.Net Core Web Api
The backend service for the Movie Search web app is a simple
.Net Core Web Api.

The only Nuget package it uses is RestSharp to facilitate
brokering the calls to the TMDB Rest api.

The "about" page has more info on the project:
[About Page](https://groover.tech/about)

It's important to set a user secret for the TMDB rest api key

```json
{
   "MovieApiKey" : "key goes here"
}
```
You will need to sign up for an API key to run the backend 
service

[TMDB Developer FAQ](https://developer.themoviedb.org/docs/faq)

# Misc
Interesting finding during testing, the movie Titanic (1997) 
made more money than an Int32 can hold: JSON integer 
2,264,162,353 is too large or small for an Int32

The default generated view model was an Int32 so I had to manually
change it to an Int64 after researching the exceptions I was getting
trying to retrieve data from the TMDB api.