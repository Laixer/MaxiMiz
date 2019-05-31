package poller

import (
	"net/http"
)

func getCPM() {
	resp, _ := http.Get("test.nl")
	println(resp)
}

// type poller interface{
// 	getCPM()
// }

// type taboolaPoller struct{

// }

// func (p taboolaPoller) getCPM(){

// }

// type googlePoller struct{

// }

// func (p googlePoller) getCPM(){

// }

// type outbrainPoller struct{

// }

// func (p outbrainPoller) getCPM(){

// }

// type revcontentPoller struct{

// }

// func (p revcontentPoller) getCPM(){

// }
