#!/bin/bash
/opt/mssql/bin/sqlservr &
PID=$!

echo "Waiting for SQL Server to start..."
sleep 20

/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -i /docker-entrypoint-initdb.d/init.sql

wait $PID