@echo off
setlocal enabledelayedexpansion
chcp 65001 >nul 2>&1
for /F "delims=#" %%E in ('"prompt #$E# & for %%E in (1) do rem" 2^>nul') do set "ESC=%%E"

set "output_file=%~dp0Project_Context.txt"
if exist "%output_file%" del "%output_file%" 2>nul

set total_files=0
for /r %%f in (*.cs) do set /a total_files+=1
if !total_files! equ 0 exit /b

set "EmptyChar=!ESC![38;2;40;40;40m░"
set "EmptyRow="
for /l %%x in (0,1,41) do set "EmptyRow=!EmptyRow!!EmptyChar!"

for /l %%y in (0,1,7) do (
    set "FullRow[%%y]="
    for /l %%x in (0,1,41) do (
        set /a "wTL=(41-%%x)*(7-%%y)", "wTR=%%x*(7-%%y)", "wBL=(41-%%x)*%%y", "wBR=%%x*%%y"
        set /a "R=(0*!wTL! + 255*!wTR! + 0*!wBL! + 255*!wBR!) / 287"
        set /a "G=(255*!wTL! + 0*!wTR! + 255*!wBL! + 255*!wBR!) / 287"
        set /a "B=(255*!wTL! + 255*!wTR! + 0*!wBL! + 0*!wBR!) / 287"
        set /a "idx=%%y*42 + %%x"
        set "c=!ESC![38;2;!R!;!G!;!B!m█"
        set "Char[!idx!]=!c!"
        set "FullRow[%%y]=!FullRow[%%y]!!c!"
    )
)

echo ^<project_context^> > "%output_file%" 2>nul
set script_count=0
set total_lines=0
cls

for /r %%f in (*.cs) do (
    set /a script_count+=1
    
    :: Conteo exacto por archivo restaurado
    for /f %%C in ('find /c /v "" ^< "%%f" 2^>nul') do set /a total_lines+=%%C
    
    set /a pct=script_count * 100 / total_files
    set /a filled=script_count * 336 / total_files
    
    <nul set /p ="!ESC![H"
    echo Procesando: !ESC![92m%%~nxf!ESC![0m!ESC![K
    echo Progreso: !ESC![93m!pct!%%!ESC![0m ^( !script_count! / !total_files! Scripts ^)!ESC![K
    echo.!ESC![K
    
    for /l %%y in (0,1,7) do (
        set /a "limit=(%%y+1)*42", "start=%%y*42"
        if !filled! geq !limit! (
            echo !FullRow[%%y]!!ESC![0m!ESC![K
        ) else if !filled! leq !start! (
            echo !EmptyRow!!ESC![0m!ESC![K
        ) else (
            set "line="
            set /a "cells=filled-start"
            for /l %%x in (0,1,41) do (
                if %%x lss !cells! (
                    set /a "idx=start+%%x"
                    for %%I in (!idx!) do set "line=!line!!Char[%%I]!"
                ) else (
                    set "line=!line!!EmptyChar!"
                )
            )
            echo !line!!ESC![0m!ESC![K
        )
    )
    
    (
        echo ^<file path="%%f"^>
        echo ```csharp
        type "%%f" 2^>nul
        echo.
        echo ```
        echo ^</file^>
        echo.
    ) >> "%output_file%" 2>nul
)

echo ^</project_context^> >> "%output_file%" 2>nul

echo.!ESC![K
echo !ESC![36m==========================================!ESC![K
echo Total de scripts (.cs): !script_count!!ESC![K
echo Total de lineas de codigo: !total_lines!!ESC![K
echo ==========================================!ESC![0m!ESC![K
pause