#!/bin/bash
# Post-template script to rename projects

ORIGINAL_NAME="TemplateAPINet10"
INFRA_ORIGINAL="TemplateaAPINet10.Infrastructure"

# Get the new project name from the first argument or use current directory
NEW_NAME="${1:-.}"
if [ "$NEW_NAME" = "." ]; then
	NEW_NAME=$(basename "$PWD")
fi

echo "Renaming projects to: $NEW_NAME"

# Rename Domain project
if [ -d "${ORIGINAL_NAME}.Domain" ]; then
	mv "${ORIGINAL_NAME}.Domain" "${NEW_NAME}.Domain"
	sed -i "s/<AssemblyName>${ORIGINAL_NAME}\.Domain<\/AssemblyName>/<AssemblyName>${NEW_NAME}.Domain<\/AssemblyName>/g" "${NEW_NAME}.Domain/${NEW_NAME}.Domain.csproj"
fi

# Rename Infrastructure project
if [ -d "$INFRA_ORIGINAL" ]; then
	mv "$INFRA_ORIGINAL" "${NEW_NAME}.Infrastructure"
	sed -i "s/<AssemblyName>${INFRA_ORIGINAL}<\/AssemblyName>/<AssemblyName>${NEW_NAME}.Infrastructure<\/AssemblyName>/g" "${NEW_NAME}.Infrastructure/${NEW_NAME}.Infrastructure.csproj"
fi

# Rename Models project
if [ -d "${ORIGINAL_NAME}.Models" ]; then
	mv "${ORIGINAL_NAME}.Models" "${NEW_NAME}.Models"
	sed -i "s/<AssemblyName>${ORIGINAL_NAME}\.Models<\/AssemblyName>/<AssemblyName>${NEW_NAME}.Models<\/AssemblyName>/g" "${NEW_NAME}.Models/${NEW_NAME}.Models.csproj"
fi

# Rename main API project
if [ -d "$ORIGINAL_NAME" ]; then
	mv "$ORIGINAL_NAME" "$NEW_NAME"
fi

# Update solution file references
if [ -f "*.sln" ]; then
	sed -i "s/${ORIGINAL_NAME}\.Domain/${NEW_NAME}.Domain/g" *.sln
	sed -i "s/${INFRA_ORIGINAL}/${NEW_NAME}.Infrastructure/g" *.sln
	sed -i "s/${ORIGINAL_NAME}\.Models/${NEW_NAME}.Models/g" *.sln
	sed -i "s/${ORIGINAL_NAME}/${NEW_NAME}/g" *.sln
fi

echo "✓ Projects renamed successfully"
