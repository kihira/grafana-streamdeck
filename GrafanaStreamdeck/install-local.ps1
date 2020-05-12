$APP_ID = "rocks.foxes.grafana-streamdeck"
$BUILD_OUTPUT = "$PSScriptRoot\bin\Debug\$APP_ID.sdPlugin\"
$STREAM_DECK_FILE = "C:\Program Files\Elgato\StreamDeck\StreamDeck.exe"

Stop-Process -Name "StreamDeck"
Start-Sleep -Second 2
Remove-Item -Path "$env:APPDATA\Elgato\StreamDeck\Plugins\$APP_ID.sdPlugin\" -Force -Recurse
Copy-Item $BUILD_OUTPUT -Destination "$env:APPDATA\Elgato\StreamDeck\Plugins\$APP_ID.sdPlugin" -Recurse
Start-Process -FilePath $STREAM_DECK_FILE
