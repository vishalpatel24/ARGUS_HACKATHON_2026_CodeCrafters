# Frontend Structure

## Framework & Tooling

| Tool | Version |
|------|---------|
| Angular | 21.2.0 |
| TypeScript | 5.9.2 |
| RxJS | 7.8.0 |
| Angular CLI | 21.2.2 |
| Node (Docker) | 20-alpine |
| Styling | SCSS |

## Architecture

Module-based Angular application (traditional `NgModule`, not standalone components).

## Directory Structure

```
CodeCrafters.FrontEnd/codecrafters-ui/
├── src/
│   ├── app/
│   │   ├── app.ts                    # Root component (uses signals)
│   │   ├── app.html                  # Root template
│   │   ├── app.scss                  # Root styles
│   │   ├── app-module.ts             # Root NgModule
│   │   ├── app-routing-module.ts     # Routing (empty routes, ready for features)
│   │   └── core/
│   │       ├── services/
│   │       │   └── api.service.ts    # Generic HTTP wrapper (get/post/put/delete)
│   │       └── interceptors/
│   │           └── api.interceptor.ts # Adds headers, handles CORS errors
│   ├── environments/
│   │   ├── environment.ts            # Dev: apiUrl = '/api'
│   │   └── environment.prod.ts       # Prod: apiUrl = '/api'
│   ├── index.html                    # HTML entry point
│   ├── main.ts                       # Bootstrap entry
│   └── styles.scss                   # Global styles
├── public/                           # Static assets
├── angular.json                      # CLI configuration
├── tsconfig.json                     # TS config (ES2022 target)
├── proxy.conf.json                   # Dev proxy: /api → localhost:5000
├── package.json                      # Dependencies & scripts
└── Dockerfile                        # Node 20 alpine, runs ng serve
```

## Key Components

### App Module (`app-module.ts`)
- Imports: `BrowserModule`, `AppRoutingModule`
- Provides: `HttpClient` with `apiInterceptor`
- Uses `provideBrowserGlobalErrorListeners()` for global error handling
- Bootstraps: `App` component

### App Component (`app.ts`)
- Selector: `app-root`
- Uses Angular signals: `title = signal('codecrafters-ui')`
- Currently renders default Angular starter template

### Routing (`app-routing-module.ts`)
- Empty routes array — ready for feature module lazy loading
- Pattern: `{ path: 'feature', loadChildren: () => import('./feature/feature.module') }`

## Core Services

### ApiService (`core/services/api.service.ts`)
- Injectable at root level
- Generic HTTP methods:
  - `get<T>(endpoint: string): Observable<T>`
  - `post<T>(endpoint: string, body: any): Observable<T>`
  - `put<T>(endpoint: string, body: any): Observable<T>`
  - `delete<T>(endpoint: string): Observable<T>`
- Constructs full URL from `environment.apiUrl`

### API Interceptor (`core/interceptors/api.interceptor.ts`)
- Functional interceptor (Angular 14+ style)
- Prepends `apiUrl` to relative URLs
- Sets `Content-Type: application/json` and `Accept: application/json`
- Logs CORS and HTTP errors to console

## Proxy Configuration (`proxy.conf.json`)

```json
{
  "/api": {
    "target": "http://localhost:5000",
    "secure": false,
    "changeOrigin": true
  }
}
```

Used in local development (`ng serve`) to forward `/api/*` requests to the backend.

## NPM Scripts

| Command | Action |
|---------|--------|
| `npm start` | `ng serve --proxy-config proxy.conf.json --open` |
| `npm run build` | `ng build` |
| `npm run watch` | `ng build --watch --configuration development` |
| `npm test` | `ng test` |

## Adding a New Feature

1. Generate module: `ng generate module features/my-feature --route my-feature --module app-module`
2. Add components inside the feature module
3. Use `ApiService` to call backend endpoints
4. Routes are lazy-loaded automatically
