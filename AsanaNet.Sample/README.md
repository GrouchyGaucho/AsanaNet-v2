# AsanaNet Sample Project

This sample project demonstrates the usage of AsanaNet library for interacting with the Asana API.

## Configuration

Before running the sample, you need to configure your Asana API credentials in `appsettings.json`:

### Personal Access Token (PAT)
To get your Personal Access Token:
1. Log in to Asana (https://app.asana.com)
2. Go to My Settings (click your profile picture)
3. Navigate to Apps > Developer apps and tokens
4. Click "Create new token"
5. Copy the token and paste it in `appsettings.json` as the `ApiToken` value

### OAuth Configuration (Optional)
If you want to test the OAuth flow:
1. Create a new OAuth app in Asana
2. Configure the OAuth settings in `appsettings.json`:
   - `ClientId`
   - `ClientSecret`
   - `RedirectUri` (default: http://localhost:3000/oauth/callback)

## Running the Sample

1. Configure your API credentials as described above
2. Build and run the project:
   ```bash
   dotnet build
   dotnet run
   ```

3. To test the OAuth flow specifically:
   ```bash
   dotnet run -- --oauth
   ``` 