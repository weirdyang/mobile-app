param(
    [string]$Owner,
    [string]$AppName,
    [string]$APIToken,
    [string]$PackagePath
)

$ErrorActionPreference = "Stop"

[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

Write-Host "Uploading file $PackagePath"

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("X-API-Token", $APIToken)
$headers.Add("Accept", "application/json")

$Uri = "https://api.mobile.azure.com/v0.1/apps/$Owner/$AppName/package_uploads"
$Result = Invoke-RestMethod -Method Post -Uri $Uri -Headers $headers -ContentType "application/json" -Verbose -Body "{}"

Write-Host "Target package upload uri $($result.upload_url)"

Invoke-WebRequest -Uri $result.upload_url -Method Post -ContentType "multipart/form-data" -InFile $PackagePath -Headers $headers

Write-Host "Package upload completed"