package config

import (
	"bufio"
	"os"
	"strconv"
	"strings"

	"github.com/google/logger"
)

func setEnvVar(key string) {
	os.Setenv(key, parseFromConfigFile(key))
}

// StringEnv checks the environment variables if a variable with the given keys exists. If yes it returns it. If no it sets it according to the config file and returns that value
func StringEnv(key string) string {
	value, ok := os.LookupEnv(key)
	if !ok {
		setEnvVar(key)
		value := StringEnv(key)
		logger.Infof("env variable %s not set. it is set to %s", key, value)
		return value
	}
	return value
}

// IntEnv checks the environment variables if a variable with the given keys exists. If yes it returns it. If no it sets it according to the config file and returns that value
func IntEnv(key string) int {
	value, ok := os.LookupEnv(key)
	if !ok {
		setEnvVar(key)
		integer := IntEnv(key)
		logger.Infof("env variable %s not set. it is set to %d", key, integer)
		return integer
	}

	integer, err := strconv.Atoi(value)
	if err != nil {
		logger.Fatalf("could not parse environment variable %s with value %s to int\n%v", key, value, err)
	}
	return integer
}

func parseFromConfigFile(key string) string {
	fileName := "/home/david/projects/Zanhos/MaxiMiz/poller/config.toml"
	file, err := os.Open(fileName)
	if err != nil {
		logger.Fatalf("could not open file %s\n%v", fileName, err)
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		if strings.Index(line, key) != -1 {
			line = strings.Trim(strings.TrimSpace(strings.Split(line, "=")[1]), "\"")
			return line
		}
	}

	logger.Fatalf("property %s not found in %s or in environment variables", key, fileName)
	return ""
}
