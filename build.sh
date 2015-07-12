#!/bin/bash
if test "$OS" = "Windows_NT"
then
  # use .Net

  .paket/paket.bootstrapper.exe
  exit_code=$?
  if [ $exit_code -ne 0 ]; then
  	exit $exit_code
  fi

  .paket/paket.exe restore
  exit_code=$?
  if [ $exit_code -ne 0 ]; then
  	exit $exit_code
  fi
  
  TARGET="KeepRunning"
  SLIDES=""

  if [ "$2" != "" ]; then
	TARGET=$1
	SLIDES=$2
  else 
	if [ "$1" != "" ]; then
	  SLIDES=$1
	else 
	  echo "please specify slides folder"
	  exit 0
	fi
  fi
  
  packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO $TARGET slides="$SLIDES" build.fsx
else
  # use mono
  mono .paket/paket.bootstrapper.exe
  exit_code=$?
  if [ $exit_code -ne 0 ]; then
  	exit $exit_code
  fi

  mono .paket/paket.exe restore
  exit_code=$?
  if [ $exit_code -ne 0 ]; then
  	exit $exit_code
  fi
  
  TARGET="KeepRunning"
  SLIDES=""

  if [ "$2" != "" ]; then
	TARGET=$1
	SLIDES=$2
  else 
	if [ "$1" != "" ]; then
	  SLIDES=$1
	else 
	  echo "please specify slides folder"
	  exit 0
	fi
  fi
  
  mono packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO $TARGET slides="$SLIDES" build.fsx
fi