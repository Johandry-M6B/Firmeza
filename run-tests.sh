#!/bin/bash

# Firmeza Test Runner Script
# This script runs tests in Docker containers

set -e

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}  Firmeza Docker Test Runner${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

# Clean up previous test results
if [ -d "test-results" ]; then
    echo -e "${YELLOW}Cleaning up previous test results...${NC}"
    # Use docker to remove files created by docker (root)
    docker run --rm -v "$(pwd):/app" -w /app alpine rm -rf test-results
fi

# Create test results directory
mkdir -p test-results

# Build and run tests
echo -e "${GREEN}Building test container...${NC}"
docker compose -f docker-compose.test.yml build

echo ""
echo -e "${GREEN}Running tests...${NC}"
docker compose -f docker-compose.test.yml up --abort-on-container-exit

# Capture exit code
TEST_EXIT_CODE=$?

# Clean up containers
echo ""
echo -e "${YELLOW}Cleaning up containers...${NC}"
docker compose -f docker-compose.test.yml down

# Display results
echo ""
echo -e "${GREEN}========================================${NC}"
if [ $TEST_EXIT_CODE -eq 0 ]; then
    echo -e "${GREEN}  ✓ Tests passed successfully!${NC}"
else
    echo -e "${RED}  ✗ Tests failed!${NC}"
fi
echo -e "${GREEN}========================================${NC}"
echo ""
echo -e "Test results are available in: ${YELLOW}./test-results/${NC}"

exit $TEST_EXIT_CODE
