# MP4 Stream – ASP.NET Core 8 MVC Application

## Overview

MP4 Stream is an ASP.NET Core 8 MVC web application that allows users to:

- View a catalogue of uploaded MP4 video files
- Upload one or more MP4 files (up to 200MB per file)
- Play any video directly within the browser
- Overwrite existing files with the same name

The application serves all functionality from a single URL:


https://localhost:{port}/


(Default route: `Home/Index`)

No authentication or authorization is required.

---

# Functional Overview

## 1. Video Catalogue

- Displays all MP4 files located in the server media folder.
- Shows:
  - File name
  - File size (in MB)
- Clicking a file:
  - Starts playback immediately
  - Stops any currently playing video
  - Ensures only one video plays at a time
- Video is displayed in a consistent 16:9 container regardless of source dimensions.

---

## 2. Upload Videos

Users can:

- Select one or more MP4 files
- Upload files up to 200MB each
- See upload progress (0–100%)
- View exact validation error messages

If a file with the same name already exists:
- The existing file is overwritten.

After successful upload:
- The application refreshes to show the updated catalogue.

If upload fails:
- The user remains on the upload screen
- A descriptive error message is displayed

---

# Validation Strategy

Validation is enforced at two levels.

## Client-Side Validation

Before calling the API, the system checks:

- At least one file is selected
- File extension is `.mp4`
- File is not empty
- File size does not exceed 200MB

This improves user experience.

## Server-Side Validation

The server enforces:

- Extension must be `.mp4`
- File must not be empty
- File must not exceed 200MB
- Business rules are strictly validated

Server validation ensures security even if client-side validation is bypassed.

---

# Upload API Rules

- Implemented using ASP.NET Core Web API (non-minimal)
- `[RequestSizeLimit(200_000_000)]` applied only to upload endpoint
- Returns proper HTTP status codes:

| Status Code | Meaning |
|-------------|----------|
| 200 | Upload successful |
| 400 | Validation error |
| 500 | Unexpected server error |

Validation errors return descriptive messages.

---

# Architecture

## Layers

### Controllers

- `HomeController`
  - Serves the main UI (`Index`)
- `UploadController`
  - Handles file uploads via Web API

### Services

- `VideoService`
  - Retrieves video catalogue
  - Performs validation
  - Saves files to disk

### Middleware

- `GlobalExceptionMiddleware`
  - Centralized error handling
  - Converts validation errors to HTTP 400
  - Converts unknown errors to HTTP 500
  - Returns JSON for API requests
  - Redirects MVC errors to error page

### Static Files

- Videos stored in:

wwwroot/media

- Served as static files
- Browser accesses videos like CSS or JS resources
- Simulates remote web server behavior

---

# Design Decisions

## 1. Middleware-Based Error Handling

A global exception middleware is used to:

- Centralize error handling
- Avoid repetitive try/catch blocks
- Ensure consistent error responses
- Improve maintainability

Controllers handle only expected validation scenarios.

---

## 2. Separation of Concerns

- Controllers handle HTTP behavior
- Services handle business logic
- Middleware handles cross-cutting concerns
- Static file middleware handles media delivery

This improves testability and maintainability.

---

## 3. Responsive UI

The UI supports widths from:

- Minimum: 400px
- Maximum: 1400px

There are:

- No horizontal scrollbars
- Consistent video sizing
- Responsive layout for mobile and desktop

---

# Non-Functional Requirements

## Performance
- Upload progress bar
- Immediate playback on selection

## Reliability
- Graceful error handling
- Proper HTTP status codes
- Validation at multiple layers

## Security
- Server-side validation enforced
- File extension validation
- File size enforcement at API level
- Safe filename handling

---

# Running the Application

## Requirements

- Visual Studio 2022
- .NET 8 SDK

## Steps

1. Open solution in Visual Studio
2. Restore NuGet packages
3. Run using:
 - IIS Express OR
 - `dotnet run`

Navigate to:


https://localhost:{port}/


---

# Project Structure


Controllers/
HomeController.cs
UploadController.cs

Services/
IVideoService.cs
VideoService.cs

Middleware/
GlobalExceptionMiddleware.cs

Views/
Home/
Index.cshtml

wwwroot/
media/
css/


---

# Potential Improvements

- Drag & drop upload support
- Video thumbnail previews
- Pagination for large catalogues
- Delete video feature
- Database-backed metadata
- Unit tests for service layer
- Cloud storage integration (Azure Blob / S3)

---

# Assumptions

- The server has write permissions to the media folder
- The application runs in a trusted environment
- Authentication is not required

---

# Time Spent

Approximately: **X–Y hours**

(Time includes architecture design, API implementation, validation logic, UI styling, responsive design, and testing.)

---

# Conclusion

This solution demonstrates:

- Clean layered architecture
- Proper HTTP semantics
- Robust validation strategy
- Middleware-based exception handling
- Static file serving
- Responsive front-end design

The goal was to build a simple yet production-quality implementation while keeping the structure maintainable and extensible.