use HotelManagementSystem;

select * from account;

select * from employee;

INSERT INTO EMPLOYEE (employee_name, gender, birthday, identify_card, address, email)
VALUES
    (N'Nguyễn Văn An', N'Nam', '1980-05-12', '123456789012', '123 Oak St', 'michael.johnson@gmail.com');

 
 EXEC proc_insertAccount
    @username = 'hello',
    @password = '123446',
    @employee_id = 2006,
	@roles = 'staff'

EXEC proc_insertAccount
    @username = 'tuyenne1',
    @password = '123446',
    @employee_id = 1007,
	@roles = 'sysadmin'

 exec proc_deleteEmployee @employee_id = 2005;