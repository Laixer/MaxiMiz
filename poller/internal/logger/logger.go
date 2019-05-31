package logger

import (
	"log"
	"time"
)

func Info(message string) {
	logMessage("info", message)
}

func Debug(message string) {
	logMessage("debug", message)
}

func Warn(message string) {
	logMessage("warn", message)
}

func Error(message string) {
	logMessage("error", message)
}

func Fatal(message string) {
	logMessage("fatal", message)
}

func logMessage(logLevel string, message string) {
	if logLevel == "fatal" {
		log.Fatal("fatal")
	} else {
		log.Printf("[%s] [%s] %s", time.Now().Format(time.RFC3339), logLevel, message)
	}
}
