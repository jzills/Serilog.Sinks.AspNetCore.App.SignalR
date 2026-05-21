#!/usr/bin/env bash

set -euo pipefail

BRANCH="$1"
VERSION="${BRANCH#release/v}"

if ! echo "$VERSION" | grep -qE '^[0-9]+\.[0-9]+\.[0-9]+$'; then
  echo "Invalid version format: $BRANCH (expected release/vX.Y.Z)" >&2
  exit 1
fi

echo "$VERSION"
