#!/usr/bin/env bash
PIDS=()
# Function to kill both processes and their children
cleanup() {
    echo -e "\nKilling all process groups..."
    for pid in "${PIDS[@]}"; do
        # The minus sign (-) kills the ENTIRE process group (parent + children + grandchildren)
        kill -9 -"$pid" 2>/dev/null
    done
    # Safety net for any detached MSBuild/Razor compiler daemons
    pkill -9 -f "Hubserver|HubClient" 2>/dev/null
    exit 0
}
# Run cleanup whenever you press Ctrl+C (SIGINT) or terminate the script
trap cleanup SIGINT SIGTERM
# 1. Run Hubserver in the CURRENT terminal
dotnet watch run --project Hubserver &
PIDS+=($!)

sleep 4
gnome-terminal --wait -- bash -c "dotnet watch run --project HubClient; exec bash" &
PIDS+=($!)

# Wait for both processes to finish
wait "${PIDS[@]}"

