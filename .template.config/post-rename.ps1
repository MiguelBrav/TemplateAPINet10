param([string]$ProjectName)

if (-not $ProjectName) {
	$ProjectName = Split-Path -Leaf (Get-Location)
}

Write-Host "Renaming projects to: $ProjectName" -ForegroundColor Green

# Rename folders
if (Test-Path "TemplateAPINet10.Domain") {
	Rename-Item "TemplateAPINet10.Domain" "$ProjectName.Domain" -Force
}

if (Test-Path "TemplateaAPINet10.Infrastructure") {
	Rename-Item "TemplateaAPINet10.Infrastructure" "$ProjectName.Infrastructure" -Force
}

if (Test-Path "TemplateAPINet10.Models") {
	Rename-Item "TemplateAPINet10.Models" "$ProjectName.Models" -Force
}

if (Test-Path "TemplateAPINet10") {
	Rename-Item "TemplateAPINet10" "$ProjectName" -Force
}

# Update .csproj files
Get-ChildItem -Filter "*.csproj" -Recurse | ForEach-Object {
	$content = Get-Content $_.FullName -Raw
	$content = $content -replace "TemplateAPINet10\.Domain", "$ProjectName.Domain"
	$content = $content -replace "TemplateaAPINet10\.Infrastructure", "$ProjectName.Infrastructure"
	$content = $content -replace "TemplateAPINet10\.Models", "$ProjectName.Models"
	$content = $content -replace "TemplateAPINet10", "$ProjectName"
	Set-Content $_.FullName -Value $content
}

# Update .sln file
Get-ChildItem -Filter "*.sln" | ForEach-Object {
	$content = Get-Content $_.FullName -Raw
	$content = $content -replace "TemplateAPINet10\.Domain", "$ProjectName.Domain"
	$content = $content -replace "TemplateaAPINet10\.Infrastructure", "$ProjectName.Infrastructure"
	$content = $content -replace "TemplateAPINet10\.Models", "$ProjectName.Models"
	$content = $content -replace "TemplateAPINet10", "$ProjectName"
	Set-Content $_.FullName -Value $content

	Rename-Item $_.FullName "$ProjectName.sln" -Force
}

Write-Host "✓ Projects renamed successfully" -ForegroundColor Green
dotnet restore
