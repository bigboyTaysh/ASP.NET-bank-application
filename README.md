## This repository implements its own banking system concept (inspired by 3D Secure) allowing secure card transactions using a security protocol.

<img src="https://user-images.githubusercontent.com/47925883/139252245-c7363b79-d9ec-47e9-84ac-7c036ef94b13.jpg" width="30%">  

1. The customer enters card details such as payment card number and code.  
2. The payment service provider communicates with the directory server by communicating its own to it API key and card data in JSON format.  
3. The directory server server checks the key and if it is valid forwards the received card data together with its API key to the banking system, also in JSON format.  
4. The banking system receives the data, checks the key and card details, and then sends back the appropriate one a message in the form of an HTTP response code or data, including card status, in the format Json to the proxy server.  
5. The directory server forwards the received data to the payment service provider.  

<img src="https://user-images.githubusercontent.com/47925883/139252644-35419ca7-3745-460d-a3bc-25f8e83c1887.jpg" width="30%">

1. The payment service provider communicates directly with the banking system by providing the API key, payment card number and order ID.  
2. The banking system checks the correctness of the transferred data, sends a request to the payment service provider for the provision of order details based on the order ID, authorizing with an API key.  
3. The billing agent checks the API key and sends back the data of the indicated order.
4. Based on the collected data, the bank displays a card payment confirmation form for the user. Before you can confirm the payment, if the user is not logged in, he must go through the process of logging into the banking system.
5. The customer confirms or rejects the payment.
6. Depending on the user's choice and the possibility of making the transaction (including whether the bank account to which the given card is connected has sufficient funds to take transaction), the banking system sends the current payment status for the specified order to the acquirer. On this basis, the payment service provider can update the payment status for the indicated order.  

# BankApplication
banking application with virtual card payment system in the [Shop](#shop) application  

<img src="https://user-images.githubusercontent.com/47925883/139247485-79a23f8e-f614-45d9-ae4e-efe07fe9bed1.jpg" width="80%">
<img src="https://user-images.githubusercontent.com/47925883/139248866-a0ea235b-92fc-49d6-a5d1-68a6617d50c2.png" width="50%">

## Modules
User:
- creating various types of accounts (payment and currency)
- making transfers
- currency exchange (download of actual buy and sell values from **NBP Web API**)
- applying for a loan
- repayment
- card payments, card management

Employee: 
- deposits, withdrawals
- approval / rejection of the loan application
- users management

Administrator:
- deposits, withdrawals
- approval / rejection of the loan application
- users management

## Technology stack
* _**ASP.NET MVC**_ 
* _**Razor**_ 
* _**Bootstrap**_
* _**jQuery**_
* _**Ajax**_
* _**MS SQL**_

# Shop
an online store application that enables card payments in [BankApplication](#bankApplication)
## Example scenario

_Adding selected products to the basket and choosing a payment method, e.g. card payment:_
<img src="https://user-images.githubusercontent.com/47925883/139249648-82874bf9-a676-45f7-8ffb-327c398bdea0.jpg" width="80%">
<img src="https://user-images.githubusercontent.com/47925883/139249750-e2d1c116-72e8-407b-bffa-12641d2b3b6f.jpg" width="80%">

_Entering the delivery address and card number with security code:_

<img src="https://user-images.githubusercontent.com/47925883/139249927-8530e065-a8a1-4036-a03b-0cfb8a8050ac.jpg" width="60%">

_Confirmation of payment in the bank after logging in:_

<img src="https://user-images.githubusercontent.com/47925883/139247716-248e3f8c-a087-4474-bb0d-758eda97e480.jpg" width="50%">

_Order summary:_  

<img src="https://user-images.githubusercontent.com/47925883/139251686-3a33bc4c-08a9-49ce-b7fe-ef004dc91912.png" width="80%">


## Technology stack
* _**ASP.NET Core Web API**_ 
* _**React**_ 
* _**Axios**_
* _**Material UI**_

# Direcotry Serwer
an application which is an intermediary server used for card payments in order to ensure security

## Technology stack
* _**ASP.NET Core Web API**_ 

