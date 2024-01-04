Create Database Olx;
use Olx

Create table Table_Admin(
admin_ID int primary key identity,
admin_NAME varchar(50) not null unique,
admin_EMAIL varchar(50) not null unique,
admin_PASSWORD varchar(50) not null 
);

Insert into Table_Admin values('Anas' , 'anas@gmail.com' , 'anas1234');
Insert into Table_Admin values('Alyan' , 'alyan@gmail.com' , 'alyan1234');
Insert into Table_Admin values('Rohaan' , 'rohaan@gmail.com' , 'rohaan1234');
Insert into Table_Admin values('Hashir' , 'hashir@gmail.com' , 'hashir1234');

select * from Table_Admin

------------------------------------------
Create table Table_Category(
category_ID  int primary key identity,
category_NAME varchar(50) not null unique,
category_IMAGE varchar(max) not  null,
category_STATUS int default 1 ,
admin_ID_foriegn_key int foreign key references Table_Admin(admin_ID)
);
---------------------------------------------------------
Create table Table_User(
userr_ID int primary key identity,
userr_NAME varchar(50) not null unique,
userr_EMAIL varchar(50) not null unique,
userr_CONTACT varchar(50) not null unique,
userr_IMAGE varchar(max) not  null,
userr_PASSWORD varchar(50) not null 
);

--------------------------------------------


Create table Table_Product(
product_ID int primary key identity,
product_NAME varchar(50) not null unique,
product_IMAGE varchar(max) not  null,
product_PRICE int not null,
product_DESCRIPTION varchar(500) not null,
category_ID_foreign_key int foreign key references Table_Category(category_ID),
userr_ID_foreign_key int foreign key references Table_User(userr_ID)

);

---------------------------------------------------------------------------------

Create Table Table_Invoice(
invoice_id int primary key identity,
inovice_date datetime,
invoice_userr_ID_foreign_key int foreign key references Table_User(userr_ID)
);

-----------------------------------------------------------------------------------

Create Table Table_Order(
order_id int primary key identity,
order_product_fk int foreign key references Table_Product(product_ID),
order_user_fk int foreign key references Table_User(userr_ID),
order_invoice_fk int foreign key references Table_Invoice(invoice_id),
order_bill float,
order_unit_price float,
order_date datetime ,
order_quantity int not null



);


select * from Table_Order

Alter Table Table_Order Drop column order_Sub_Total