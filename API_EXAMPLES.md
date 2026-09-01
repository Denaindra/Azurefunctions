# API Examples - HttpTriggerGPT

This document provides comprehensive examples of how to use the enhanced HttpTriggerGPT function with public API calls.

## Function Overview

The `HttpTriggerGPT` function now:
- ✅ Makes calls to the JSONPlaceholder public API
- ✅ Retrieves user data based on userId parameter
- ✅ Returns structured JSON responses
- ✅ Handles errors gracefully
- ✅ Logs requests and responses for monitoring

## API Endpoint

```
GET/POST https://connectWithGPT.azurewebsites.net/api/HttpTriggerGPT?userId={id}&code={function-key}
```

## Query Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `userId` | integer | No | User ID to fetch (1-10). Defaults to 1 |
| `code` | string | Yes | Function authorization key |

## Example 1: Basic Request (Default User)

### Request
```bash
curl "https://connectWithGPT.azurewebsites.net/api/HttpTriggerGPT?code=YOUR_FUNCTION_KEY"
```

### Response
```json
{
  "timestamp": "2026-09-01T21:47:59Z",
  "userId": "1",
  "userData": {
    "id": 1,
    "name": "Leanne Graham",
    "username": "Bret",
    "email": "Sincere@april.biz",
    "address": {
      "street": "Kulas Light",
      "suite": "Apt. 556",
      "city": "Gwenborough",
      "zipcode": "92998-3874",
      "geo": {
        "lat": "-37.3159",
        "lng": "81.1496"
      }
    },
    "phone": "1-770-736-8031 x56442",
    "website": "hildegard.org",
    "company": {
      "name": "Romaguera-Crona",
      "catchPhrase": "Multi-layered client-server neural-net",
      "bs": "harness real-time e-markets"
    }
  },
  "message": "Successfully retrieved user data from public API",
  "source": "JSONPlaceholder API"
}
```

## Example 2: Specific User Request

### Request
```bash
curl "https://connectWithGPT.azurewebsites.net/api/HttpTriggerGPT?userId=5&code=YOUR_FUNCTION_KEY"
```

### Response
```json
{
  "timestamp": "2026-09-01T21:48:15Z",
  "userId": "5",
  "userData": {
    "id": 5,
    "name": "Chelsey Dietrich",
    "username": "Kamren",
    "email": "Lucio_Hettinger@april.com",
    "address": {
      "street": "Skiles Walks",
      "suite": "Suite 351",
      "city": "Roscoe",
      "zipcode": "33263",
      "geo": {
        "lat": "-31.8129",
        "lng": "62.5342"
      }
    },
    "phone": "(254)954-1289",
    "website": "demarco.info",
    "company": {
      "name": "Keebler LLC",
      "catchPhrase": "User-centric fault-tolerant solution",
      "bs": "revolutionize end-to-end systems"
    }
  },
  "message": "Successfully retrieved user data from public API",
  "source": "JSONPlaceholder API"
}
```

## Example 3: Local Development Testing

### Start Local Function
```bash
func start
```

### Test with cURL
```bash
curl "http://localhost:7071/api/HttpTriggerGPT?userId=3"
```

### Test with PowerShell
```powershell
$url = "http://localhost:7071/api/HttpTriggerGPT?userId=3"
$response = Invoke-WebRequest -Uri $url -Method Get
$response.Content | ConvertFrom-Json | ConvertTo-Json
```

### Test with Python
```python
import requests
import json

url = "http://localhost:7071/api/HttpTriggerGPT"
params = {"userId": "3"}
response = requests.get(url, params=params)
data = response.json()
print(json.dumps(data, indent=2))
```

### Test with JavaScript/Node.js
```javascript
const userId = 3;
const url = `http://localhost:7071/api/HttpTriggerGPT?userId=${userId}`;

fetch(url)
  .then(response => response.json())
  .then(data => console.log(JSON.stringify(data, null, 2)))
  .catch(error => console.error('Error:', error));
```

## Example 4: POST Request

### Request
```bash
curl -X POST "https://connectWithGPT.azurewebsites.net/api/HttpTriggerGPT?userId=7&code=YOUR_FUNCTION_KEY"
```

### Response (Same as GET)
```json
{
  "timestamp": "2026-09-01T21:48:45Z",
  "userId": "7",
  "userData": { ... },
  "message": "Successfully retrieved user data from public API",
  "source": "JSONPlaceholder API"
}
```

## Example 5: Error Handling

### Invalid User ID (Out of Range)
```bash
curl "http://localhost:7071/api/HttpTriggerGPT?userId=999"
```

### Response
```json
{
  "error": "User not found"
}
```

### Missing Function Key (Azure)
```bash
curl "https://connectWithGPT.azurewebsites.net/api/HttpTriggerGPT?userId=1"
```

### Response
```
Unauthorized
```

## Example 6: Advanced Usage - Batch Processing

### PowerShell Script to Fetch Multiple Users
```powershell
$functionKey = "YOUR_FUNCTION_KEY"
$baseUrl = "https://connectWithGPT.azurewebsites.net/api/HttpTriggerGPT"
$users = @()

for ($i = 1; $i -le 5; $i++) {
    $url = "$baseUrl`?userId=$i&code=$functionKey"
    try {
        $response = Invoke-WebRequest -Uri $url -Method Get
        $userData = $response.Content | ConvertFrom-Json
        $users += $userData
        Write-Host "✓ Fetched user $i"
    }
    catch {
        Write-Host "✗ Error fetching user $i: $_"
    }
}

# Export results to JSON file
$users | ConvertTo-Json | Out-File -FilePath "users_data.json"
Write-Host "Exported $($users.Count) users to users_data.json"
```

## Example 7: Integration with Azure Logic Apps

### Logic App HTTP Action Configuration

**Method**: GET

**URI**: 
```
@{
  concat(
    'https://connectWithGPT.azurewebsites.net/api/HttpTriggerGPT?userId=',
    triggerBody()['userId'],
    '&code=YOUR_FUNCTION_KEY'
  )
}
```

**Headers**:
```json
{
  "Content-Type": "application/json"
}
```

**Parse JSON Configuration**:
```json
{
  "type": "object",
  "properties": {
    "timestamp": {
      "type": "string"
    },
    "userId": {
      "type": "string"
    },
    "userData": {
      "type": "object",
      "properties": {
        "id": {
          "type": "integer"
        },
        "name": {
          "type": "string"
        },
        "email": {
          "type": "string"
        }
      }
    },
    "message": {
      "type": "string"
    }
  }
}
```

## Example 8: Response Headers

```
Content-Type: application/json; charset=utf-8
Date: Wed, 01 Sep 2026 21:47:59 GMT
Server: Kestrel
Transfer-Encoding: chunked
Connection: keep-alive
Request-Context: appId=cid-v1:xxxxxxxx
```

## Example 9: Performance Metrics

### Request Execution Time
- **Cold Start**: ~1.5-2s (first request, includes Azure startup)
- **Warm Start**: ~200-300ms (subsequent requests)
- **API Call Time**: ~50-150ms (JSONPlaceholder API)
- **Total Response**: ~250-400ms (typical warm response)

## Example 10: Monitoring with Application Insights

### Query in Application Insights
```kusto
requests
| where name == "HttpTriggerGPT"
| summarize count(), avg(duration), max(duration) by bin(timestamp, 1m)
```

### Custom Event Tracking
```kusto
customEvents
| where name == "UserDataFetched"
| extend userId = tostring(customDimensions.userId)
| summarize count() by userId
```

## Available User IDs

The JSONPlaceholder API provides 10 mock users (IDs 1-10):

| ID | Name | Company | Email |
|----|------|---------|-------|
| 1 | Leanne Graham | Romaguera-Crona | Sincere@april.biz |
| 2 | Ervin Howell | Deckow-Cyst | Shanna@melissa.tv |
| 3 | Clementine Bauch | Romaguera-Jacobson | Nathan@yesenia.net |
| 4 | Patricia Lebsack | Robel-Corkery | Julianne.OConner@kory.com |
| 5 | Chelsey Dietrich | Keebler LLC | Lucio_Hettinger@april.com |
| 6 | Mrs. Dennis Schulist | Considine-Lockman | Karley_Dach@jasper.info |
| 7 | Kurtis Weissnat | The Midwest Group | Telly.Hoeger@billy.biz |
| 8 | Nicholas Runolfsdottir V | Balistreri, Schaefer and Balistreri | Sherwood@rosamond.me |
| 9 | Glenna Reichert | Yost and Sons | Chaim_McDermott@dana.biz |
| 10 | Clementina DuBuque | Hoeger LLC | Rey.Padberg@karina.biz |

## Security Considerations

⚠️ **Important**: 
- Always use the `code` parameter for authorization
- Never expose function keys in client-side code
- Use Managed Identities in Azure for service-to-service authentication
- Implement rate limiting for production use
- Validate and sanitize all user inputs

## Error Response Codes

| Status Code | Meaning |
|------------|---------|
| 200 | Success - User data retrieved |
| 404 | Not Found - User ID doesn't exist |
| 503 | Service Unavailable - External API unreachable |
| 500 | Internal Server Error - Unexpected error |
| 401 | Unauthorized - Missing or invalid function key |

## Next Steps

1. Deploy to Azure using: `func azure functionapp publish connectWithGPT`
2. Get your function key from Azure Portal
3. Replace `YOUR_FUNCTION_KEY` with actual key
4. Start making requests to your live function!

For more information, see [README.md](./README.md)
