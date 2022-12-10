#!/bin/bash

DIR="$(dirname "${BASH_SOURCE[0]}")"
DIR="$(realpath "${DIR}")"

RESULTS_FOLDER=$DIR/../.specflow_results
mkdir -p $RESULTS_FOLDER

for f in $DIR/*; do
    if [ -d "$f" ]; then
        assembly="$(basename $f)"
        sedScript='s@"FeatureFolderPath":"@"FeatureFolderPath":"'$assembly'/@g'
        sed -e $sedScript $DIR/$assembly/bin/TestExecution.json > $RESULTS_FOLDER/TestExecution_$assembly.json
    fi
done

dotnet livingdoc feature-folder $DIR --title "Smusdi core" -o $RESULTS_FOLDER -t $RESULTS_FOLDER/TestExecution*.json
