package logger

import (
	"log"
	"time"
)

// Info ...
func Info(message string) {
	logMessage("info", message)
}

// Debug ...
func Debug(message string) {
	logMessage("debug", message)
}

// Warn ...
func Warn(message string) {
	logMessage("warn", message)
}

// Error ...
func Error(message string) {
	logMessage("error", message)
}

// Fatal ...
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
