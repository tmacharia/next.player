# Testing

## Config Settings File

TheMovieDb API requires a key, meaning tmdb tests will fail until a key is provided. If you don't have a tmdb api key, you can [request one here](https://www.themoviedb.org/documentation/api). 

All config settings are managed in one single settings file.
Therefore, at the root of the `Tests` folder, make sure to create a file named `settings.json`. Inside the json file, add your tmdb key as shown below.

```json
{
    "TmdbApiKey": "{TMDB_API_KEY_HERE}"
}
```

> The file `settings.json` is included in .gitignore rules to make sure that your personal API keys are safe and wont be shared when you commit code.