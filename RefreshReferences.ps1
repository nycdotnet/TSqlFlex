if (!(Test-Path -Path "References")) {
	New-Item -ItemType directory -Path "References";
}

if (!(Test-Path -Path "References\RedGate.AppHost")) {
	New-Item -ItemType directory -Path "References\RedGate.AppHost";
}

if (!(Test-Path -Path "References\RedGate.AppHost") ){
	New-Item -ItemType directory -Path "References\RedGate.AppHost";
}

DEL "References\RedGate.AppHost\*.*";

COPY "..\RedGate.AppHost\Build\Debug\*.*" "References\RedGate.AppHost\"


