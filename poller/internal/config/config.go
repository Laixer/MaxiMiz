package config

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"

	"github.com/google/logger"
)

//TODO this should be passed as an argument
const configFileLocation = "example_config.toml"

func setEnvVar(key string) error {
	value, err := parseFromConfigFile(key)
	if err != nil {
		return err
	}
	os.Setenv(key, value)
	return nil
}

// StringEnv checks the environment variables if a variable with the given keys exists. If yes it returns it. If no it sets it according to the config file and returns that value
func StringEnv(key string) string {
	value, ok := os.LookupEnv(key)
	if !ok {
		err := setEnvVar(key)
		if err != nil {
			return ""
		}
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
		err := setEnvVar(key)
		if err != nil {
			return -1
		}
		integer := IntEnv(key)
		logger.Infof("env variable %s not set. it is set to %d", key, integer)
		return integer
	}

	integer, err := strconv.Atoi(value)
	if err != nil {
		logger.Errorf("could not parse environment variable %s with value %s to int\n%v", key, value, err)
	}
	return integer
}

func parseFromConfigFile(key string) (string, error) {
	//TODO make this a final const that is relative
	file, err := os.Open(configFileLocation)
	if err != nil {
		logger.Errorf("could not open file %s\n%v", configFileLocation, err)
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		if strings.Index(line, key) != -1 {
			line = strings.Trim(strings.TrimSpace(strings.Split(line, "=")[1]), "\"")
			if line == "" {
				logger.Fatalf("value %s in config file %s was empty", key, configFileLocation)
			}
			return line, nil
		}
	}

	return "", fmt.Errorf("property %s not found in %s or in environment variables", key, configFileLocation)
}
