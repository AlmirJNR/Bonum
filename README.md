# Bonum
#### _means "Good" in latin_

## What it is?

A http api with a tesseract ocr wrapper using containers with integrated xunit tests

## How to run it?

Pre-requisites (run only):
- Docker and docker-compose
- Any http request tool (optional)(swagger is available if needed)

Steps:
1. Run `docker compose up` in your terminal
2. Access on your browser the address [http://localhost:5000/swagger](http://localhost:5000/swagger)
3. Use the swagger interface to send any supported image file to receive the extracted text as the response

Pre-requisites (development):
- Dotnet SDK 6.0.+
- Any IDE that works with Dotnet