<?php

switch ($_REQUEST["server"]) {

            case "check":print("PHP Confirmed"); break;
            case "send": {


                
                $to = $_POST["to"];
                $headers = 'From:'. $_POST["email"];
                $subject = $_POST["subject"];
				
                $messageBody = "";
                
                
                foreach ($_POST as $key => $value){
                    if ($key != 'to' && $key != 'smtp' && $key != 'email' && $key != 'subject'){
	
						$messageBody .= '<b>'.str_replace('_',' ',$key).'</b>:<br/>';
	
						$messageBody .= ''.stripslashes($value).'<br/>';
					}
                }

               if (mail($to,$subject,$messageBody,$headers))  print("2"); else print("3");
                
            }   break;
            default: print("1"); break;
}
?>