$requestCount = 1000000

1..$requestCount | ForEach-Object -Parallel {
    curl "http://localhost:5180/metrics" > $null
} -ThrottleLimit $requestCount
