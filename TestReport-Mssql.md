## Performance Test Report

Provider: MSSQL

Test time: 2022/07/28 08:19:24

Customer: 1,000,000

Product: 100,000

Order: 1,000,000

Bellow is test performance report. Lower is better

|Test case|Constraint|Non Constraint|
|--|--|--|
|Search Product|2ms|3ms|
|Search Product|2ms|2ms|
|Search Product|2ms|1ms|
|Search Product|2ms|1ms|
|Search Product|1ms|1ms|
|Avg|1.8ms|1.6ms|
|Get revenue last month|294ms|336ms|
|Get revenue last month|283ms|334ms|
|Get revenue last month|291ms|291ms|
|Get revenue last month|297ms|285ms|
|Get revenue last month|281ms|289ms|
|Avg|289.2ms|307ms|
|Get revenue this year|386ms|352ms|
|Get revenue this year|342ms|402ms|
|Get revenue this year|338ms|384ms|
|Get revenue this year|376ms|375ms|
|Get revenue this year|340ms|341ms|
|Avg|356.4ms|370.8ms|
|Best seller product this year|413ms|379ms|
|Best seller product this year|402ms|387ms|
|Best seller product this year|379ms|437ms|
|Best seller product this year|380ms|428ms|
|Best seller product this year|386ms|379ms|
|Avg|392ms|402ms|
|Top customer this year|467ms|431ms|
|Top customer this year|568ms|516ms|
|Top customer this year|508ms|531ms|
|Top customer this year|563ms|466ms|
|Top customer this year|691ms|445ms|
|Avg|559.4ms|477.8ms|
|Insert customer|7ms|6ms|
|Insert customer|9ms|6ms|
|Insert customer|9ms|6ms|
|Insert customer|11ms|6ms|
|Insert customer|9ms|6ms|
|Avg|9ms|6ms|
|Update customer|11ms|12ms|
|Update customer|12ms|11ms|
|Update customer|14ms|10ms|
|Update customer|15ms|10ms|
|Update customer|10ms|10ms|
|Avg|12.4ms|10.6ms|
|Delete customer|13ms|11ms|
|Delete customer|180ms|9ms|
|Delete customer|12ms|10ms|
|Delete customer|12ms|10ms|
|Delete customer|10ms|9ms|
|Avg|45.4ms|9.8ms|
