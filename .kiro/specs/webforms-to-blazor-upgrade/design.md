# Design Document

## Overview

This design outlines the migration strategy from a .NET Framework 4.6 WebForms application to a modern .NET 9 Blazor Server application. The migration will preserve all existing functionality while modernizing the architecture to use component-based design patterns, dependency injection, and modern .NET conventions.

The approach focuses on a complete rewrite using Blazor Server rather than attempting incremental migration, as the applications are small enough to make this feasible while ensuring a clean, maintainable result.

Documentation to help:
- https://learn.microsoft.com/en-us/dotnet/architecture/blazor-for-web-forms-developers/migration
- https://learn.microsoft.com/en-us/aspnet/core/migration/fx-to-core/?view=aspnetcore-9.0

## Architecture

### High-Level Architecture

The new application will follow the standard .NET 9 Blazor Server architecture:

```
┌─────────────────────────────────────────┐
│              Browser                    │
│  ┌─────────────────────────────────────┐│
│  │         Blazor UI                   ││
│  │  ┌─────────────┐ ┌─────────────────┐││
│  │  │ Components  │ │   JavaScript    │││
│  │  │             │ │   Interop       │││
│  │  └─────────────┘ └─────────────────┘││
│  └─────────────────────────────────────┘│
└─────────────────────────────────────────┘
                    │
              SignalR Connection
                    │
┌─────────────────────────────────────────┐
│           .NET 9 Server                 │
│  ┌─────────────────────────────────────┐│
│  │        Blazor Server                ││
│  │  ┌─────────────┐ ┌─────────────────┐││
│  │  │   Pages/    │ │    Services     │││
│  │  │ Components  │ │                 │││
│  │  └─────────────┘ └─────────────────┘││
│  └─────────────────────────────────────┘│
│  ┌─────────────────────────────────────┐│
│  │         ASP.NET Core                ││
│  │  ┌─────────────┐ ┌─────────────────┐││
│  │  │ Middleware  │ │   Static Files  │││
│  │  │   Pipeline  │ │                 │││
│  │  └─────────────┘ └─────────────────┘││
│  └─────────────────────────────────────┘│
└─────────────────────────────────────────┘
```

### Project Structure

The new project will follow modern .NET 9 conventions:

```
SimpleBlazorApp/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   ├── MainLayout.razor.css
│   │   └── NavMenu.razor
│   └── Pages/
│       ├── Home.razor
│       ├── About.razor
│       └── Contact.razor
├── Services/
│   └── GreetingService.cs
├── wwwroot/
│   ├── css/
│   │   ├── bootstrap/
│   │   └── site.css
│   ├── js/
│   └── favicon.ico
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── SimpleBlazorApp.csproj
```

## Components and Interfaces

### Core Components

#### 1. MainLayout Component
- **Purpose**: Provides the main application layout structure
- **Responsibilities**: 
  - Render navigation menu
  - Provide content area for page components
  - Include Bootstrap CSS and JavaScript references
- **Key Features**:
  - Responsive Bootstrap navigation
  - Footer with copyright information
  - Consistent styling across all pages

#### 2. NavMenu Component
- **Purpose**: Handles application navigation
- **Responsibilities**:
  - Render navigation links for Home, About, Contact
  - Highlight active page
  - Support responsive mobile navigation
- **Key Features**:
  - Bootstrap navbar implementation
  - Active link highlighting
  - Mobile-friendly collapsible menu

#### 3. Home Page Component
- **Purpose**: Main landing page with greeting functionality
- **Responsibilities**:
  - Display welcome content
  - Provide greeting form with name input
  - Handle form submission and display results
  - Maintain form state during user session
- **Key Features**:
  - Two-way data binding for name input
  - Server-side form processing
  - XSS protection through automatic HTML encoding
  - State preservation

#### 4. About Page Component
- **Purpose**: Static informational page
- **Responsibilities**:
  - Display about information
  - Maintain consistent layout with other pages

#### 5. Contact Page Component
- **Purpose**: Contact information display
- **Responsibilities**:
  - Display contact details
  - Maintain consistent layout with other pages

### Services

#### GreetingService
- **Purpose**: Handle greeting business logic
- **Interface**:
  ```csharp
  public interface IGreetingService
  {
      string GenerateGreeting(string name);
      bool ValidateName(string name);
  }
  ```
- **Responsibilities**:
  - Validate user input
  - Generate appropriate greeting messages
  - Handle empty/null name scenarios
  - Provide XSS protection

## Data Models

### GreetingModel
```csharp
public class GreetingModel
{
    public string Name { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsValid { get; set; } = true;
}
```

### Navigation Models
```csharp
public class NavigationItem
{
    public string Text { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
```

## Error Handling

### Global Error Handling
- **Error Boundary**: Implement Blazor error boundaries to catch and handle component-level errors
- **Global Exception Handler**: Configure global exception handling in Program.cs
- **User-Friendly Error Pages**: Create custom error pages for different error scenarios

### Logging Strategy
- **Logging Provider**: Use built-in .NET logging with console and debug providers
- **Log Levels**: Configure appropriate log levels for development and production
- **Structured Logging**: Use structured logging for better error tracking

### Error Scenarios
1. **Component Rendering Errors**: Handle with error boundaries
2. **Service Exceptions**: Catch and log in service layer, return appropriate error responses
3. **Validation Errors**: Display user-friendly validation messages
4. **Network Errors**: Handle SignalR connection issues gracefully

## Testing Strategy

### Unit Testing
- **Framework**: xUnit with Microsoft.Extensions.Testing
- **Coverage Areas**:
  - GreetingService business logic
  - Component logic (code-behind methods)
  - Validation logic
  - Error handling scenarios

### Integration Testing
- **Framework**: ASP.NET Core Test Host
- **Coverage Areas**:
  - Full page rendering
  - Navigation functionality
  - Form submission workflows
  - Service integration

### Component Testing
- **Framework**: bUnit for Blazor component testing
- **Coverage Areas**:
  - Component rendering
  - Event handling
  - Parameter binding
  - State management

## Migration Strategy

### Phase 1: Project Setup
1. Create new .NET 9 Blazor Server project
2. Configure project structure and dependencies
3. Set up basic routing and layout

### Phase 2: Static Content Migration
1. Migrate About and Contact pages (static content)
2. Implement navigation structure
3. Apply Bootstrap styling

### Phase 3: Interactive Features Migration
1. Migrate Home page with greeting functionality
2. Implement GreetingService
3. Add form handling and state management

### Phase 4: Styling and Polish
1. Ensure pixel-perfect match with original design
2. Implement responsive behavior
3. Add error handling and validation

### Phase 5: Testing and Validation
1. Comprehensive testing of all functionality
2. Performance validation
3. Cross-browser compatibility testing

## Technology Decisions

### Framework Choice: Blazor Server vs Blazor WebAssembly
**Decision**: Blazor Server
**Rationale**: 
- Maintains server-side processing similar to original WebForms
- Better performance for simple applications
- Easier migration path from WebForms server-side model
- No need for client-side .NET runtime download

### State Management
**Decision**: Component-level state with services for business logic
**Rationale**:
- Simple application doesn't require complex state management
- Component state sufficient for form data
- Services provide testable business logic separation

### CSS Framework
**Decision**: Keep Bootstrap 5.2.3
**Rationale**:
- Already used in original application
- Maintains visual consistency
- Well-supported in Blazor applications
- Responsive design capabilities

### Dependency Injection
**Decision**: Built-in .NET DI container
**Rationale**:
- Sufficient for application needs
- Standard .NET approach
- Good performance characteristics
- Easy testing support

## Security Considerations

### XSS Protection
- Blazor automatically HTML-encodes content by default
- Explicit encoding for any raw HTML scenarios
- Input validation on all user inputs

### CSRF Protection
- Built-in ASP.NET Core CSRF protection
- Blazor forms automatically include anti-forgery tokens

### Content Security Policy
- Configure appropriate CSP headers
- Restrict inline scripts and styles where possible

### HTTPS Enforcement
- Enforce HTTPS in production
- Configure HSTS headers
- Secure cookie settings

## Performance Considerations

### SignalR Optimization
- Configure appropriate connection timeouts
- Implement connection retry logic
- Monitor connection health

### Static File Optimization
- Enable static file compression
- Configure appropriate caching headers
- Optimize CSS and JavaScript bundling

### Component Performance
- Use appropriate component lifecycle methods
- Implement efficient re-rendering strategies
- Minimize unnecessary state changes

## Deployment Considerations

### Configuration Management
- Use appsettings.json for configuration
- Environment-specific configuration files
- Secure sensitive configuration data

### Static File Handling
- Configure static file middleware
- Implement appropriate caching strategies
- Optimize file serving performance

### Health Checks
- Implement basic health check endpoints
- Monitor application health
- Configure appropriate health check responses