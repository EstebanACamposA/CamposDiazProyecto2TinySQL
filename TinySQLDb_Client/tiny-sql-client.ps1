function Execute-MyQuery
{
    param
    (
        [string]$QueryFile,
        [int]$Port,
        [string]$IP
    )

    # Verificar si el archivo de consultas existe
    if (-not (Test-Path $QueryFile))
    {
        Write-Error "El archivo de consulta no existe: $QueryFile"
        return
    }

    # Leer las consultas del archivo
    $queries = Get-Content -Path $QueryFile -Raw
    $queriesArray = $queries -split ';'

    try
    {
        # Crear conexión TCP al servidor
        $client = New-Object System.Net.Sockets.TcpClient
        $client.Connect($IP, $Port)

        # Obtener el stream para leer y escribir datos
        $stream = $client.GetStream()
        $writer = New-Object System.IO.StreamWriter($stream)
        $reader = New-Object System.IO.StreamReader($stream)

        foreach ($query in $queriesArray)
        {
            $query = $query.Trim()

            if (-not [string]::IsNullOrWhiteSpace($query))
            {
                try
                {
                    # Medir el tiempo de ejecución
                    $startTime = Get-Date

                    # Enviar la consulta al servidor
                    $writer.WriteLine($query)
                    $writer.Flush()

                    # Leer la respuesta del servidor
                    $response = $reader.ReadLine()

                    $endTime = Get-Date
                    $duration = $endTime - $startTime

                    # Mostrar los resultados en formato tabla
                    Write-Host "Server Response: $response"
                    Write-Host "Execution time: $($duration.TotalMilliseconds) ms"
                } catch
                {
                    Write-Error "Error while executing query: $_"
                }
            }
        }

        # Cerrar conexión
        $writer.Close()
        $reader.Close()
        $client.Close()

    } catch
    {
        Write-Error "No se pudo conectar al servidor TCP: $_"
    }
}
