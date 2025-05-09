# Backend
`dotnet run` it should then listen on port 5137, you can test by going to http://localhost:5137/api/invoke, the request will take 10 seconds to complete, you will see a `.` in the console when the request starts, and a `!` in the console when the request ends.

# bff_dotnet
`dotnet run`

# bff_node
`npm install` (once off)
`node index.js`

# bff_python
`uvicorn main:app`
`uvicorn main:app --workers 4`

# loadgen
`dotnet run direct 500` - load test backend directly
`dotnet run python 500` - load test via python bff
`dotnet run nodejs 500` - load test via nodejs bff
`dotnet run dotnet 500` - load test via dotnet bff