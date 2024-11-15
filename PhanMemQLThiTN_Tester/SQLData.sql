use master 
go
create database QLThiTN
go
use QLThiTN
go

create table monhoc(
	MaMonHoc char(10) primary key not null,
    TenMonHoc nvarchar(50) NOT NULL
);

create table chuongcacmonhoc_(
	MaChuong char(10) primary key not null,
    TenChuong nvarchar(255) NOT NULL,
    MaMonHoc char(10) not null,
    foreign key (MaMonHoc) references monhoc(MaMonHoc)
);

create table CacCauHoi(
	MaCauHoi char(10) primary key not null,
    NoiDungCauHoi nvarchar(255) NOT NULL,
    DapAnA nvarchar(255) NOT NULL,
    DapAnB nvarchar(255) NOT NULL,
    DapAnC nvarchar(255) NOT NULL,
    DapAnD nvarchar(255) NOT NULL,
	CauTraLoi nvarchar(225) not null,
    MaChuong char(10) not null,
    foreign key (MaChuong) references chuongcacmonhoc_(MaChuong)
);
 CREATE TABLE HOCVIEN(
	MAHV CHAR(13) NOT NULL,
	HOLOT NVARCHAR(50) NOT NULL,
	TEN NVARCHAR(20) NOT NULL,
	PHAI NCHAR(3) NOT NULL,
	SODT CHAR(10),
	CONSTRAINT PK_HV PRIMARY KEY(MAHV)
);
ALTER TABLE HOCVIEN
ADD MALOP CHAR(10)

CREATE TABLE LOPHOC
(
	MALOP CHAR(10) NOT NULL,
	TENLOP NVARCHAR(50) NOT NULL UNIQUE,
	CONSTRAINT PK_LH PRIMARY KEY(MALOP)
)
CREATE TABLE LOP_MONHOC
(
	MALOP CHAR(10),
	MaMonHoc CHAR(10),
	CONSTRAINT PK_LOP_MH PRIMARY KEY(MALOP, MaMonHoc),
	CONSTRAINT FK_LOP_MH_MH FOREIGN KEY (MaMonHoc) REFERENCES MONHOC(MaMonHoc),
	CONSTRAINT FK_LOP_MH_LOP FOREIGN KEY (MALOP) REFERENCES LOPHOC(MALOP)
)

--select * from LOPHOC
--select LOP_MONHOC.MALOP from LOP_MONHOC, LOPHOC where LOP_MONHOC.MALOP = LOPHOC.MALOP
ALTER TABLE HOCVIEN
ADD CONSTRAINT FK_HV_LOP FOREIGN KEY(MALOP) REFERENCES LOPHOC(MALOP)
--
CREATE TABLE DETHI(
	MADETHI CHAR(10) NOT NULL,
	MaMonHoc CHAR(10) NOT NULL,
	CONSTRAINT PK_DETHI PRIMARY KEY(MADETHI),
	CONSTRAINT FK_DETHI_MH FOREIGN KEY(MaMonHoc) REFERENCES MONHOC(MaMonHoc)
)
CREATE TABLE DETHI_QLADMIN(
	MADETHI CHAR(10) NOT NULL,
	TENDETHI NVARCHAR(200) NOT NULL,
	SOLUONGCAUHOI CHAR(5) NOT NULL, 
	TGLAMBAI CHAR(10) NOT NULL,
	MALOP CHAR(12) NOT NULL,
	MONTHI NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_DETHI_QLADMIN PRIMARY KEY(MADETHI),
)
CREATE TABLE CHITIET_DETHI(
	MADETHI CHAR(10),
	MACAUHOI CHAR(10),
	CONSTRAINT PK_CTDT PRIMARY KEY(MADETHI, MACAUHOI),
	CONSTRAINT FK_CTDT_DT FOREIGN KEY(MADETHI) REFERENCES DETHI(MADETHI),
	CONSTRAINT FK_CTDT_CH FOREIGN KEY(MACAUHOI) REFERENCES CacCauHoi(MACAUHOI)
)

CREATE TABLE KETQUA(
	IDKETQUA CHAR(20),-- Note char 20
	MADETHI CHAR(10),
	MAHV CHAR(13),
	TONGSODIEM FLOAT,
	CONSTRAINT PK_KQ PRIMARY KEY(IDKETQUA),
	CONSTRAINT FK_KQ_HV FOREIGN KEY(MAHV) REFERENCES HOCVIEN(MAHV),
	CONSTRAINT FK_KQ_DT FOREIGN KEY(MADETHI) REFERENCES DETHI(MADETHI)
)
CREATE TABLE CT_KETQUA
(
	IDKETQUA CHAR(20), -- Note char 20
	MACAUHOI CHAR(10),
	CAUTRALOI NVARCHAR(200),
	CauTraLoiCuaHV char(10),
	CONSTRAINT PK_CT_KQ PRIMARY KEY(IDKETQUA, MACAUHOI),
	CONSTRAINT FK_CT_KQ_CH FOREIGN KEY(MACAUHOI) REFERENCES CacCauHoi(MACAUHOI),
	CONSTRAINT FK_CT_KQ_KQ FOREIGN KEY(IDKETQUA) REFERENCES KETQUA(IDKETQUA)
)
-- them cac mon hoc
insert into monhoc(MaMonHoc,TenMonHoc)
values ('71TO',N'Toán học'),('71VL',N'Vật lý'),('71SH',N'Sinh học'),('71LS',N'Lịch sử'),('71DL',N'Địa lí'),('71GDCD',N'Giáo dục công dân');

-- thêm chương vào các môn học
insert into chuongcacmonhoc_(MaChuong,TenChuong,MaMonHoc)
 values('TO01',N'Đại số', '71TO'),
	   ('TO02',N'Hình học', '71TO'),
	   ('VL01',N'Cơ học','71VL'),	 
	   ('VL02',N'Quang học', '71VL'),
	   ('SH01',N'Di truyền học', '71SH'),
       ('SH02',N'Tiến hóa', '71SH'),
	   ('LS01',N'Lịch sử thế giới cận đại', '71LS'),
	   ('LS02',N'Lịch sử Việt Nam từ Lịch sử Việt Nam từ năm 1919 đến năm 2000', '71LS'),
	   ('DL01',N'Địa lý dân cư', '71DL'),
	   ('DL02',N'Địa lý kinh tế', '71DL'),
	   ('GDCD01',N'Pháp luật và đời sống', '71GDCD'),
	   ('GDCD02',N'Thực hiện pháp luật', '71GDCD');


-- them vao cau hoi
insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('101',N'Giải phương trình sau : 2x + 4 =8 ?','A.4','B.3','C.2','D.1',N'Đáp án chính xác C','TO01');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values ('102',N'Khối đa diện có các mặt là những tam giác thì:',N'A.Số mặt và số đỉnh của nó bằng nhau',
 N'B. Số mặt và số cạnh của nó bằng nhau',
 N'C. Số mặt của nó là một số chẵn',N'D. Số mặt của nó là một số lẻ',N'Đáp án chính xác C','TO02');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values ('103',N'Trong trường hợp nào dưới đây có thể coi một đoàn tàu như một chất điểm?',
	N'A. Đoàn tàu lúc khởi hành.', N'B. Đoàn tàu đang qua cầu.',
    N'C. Đoàn tàu đang chạy trên một đoạn đường vòng.', N'D. Đoàn tàu đang chạy trên đường Hà Nội -Vinh.',N'Đáp án chính xác D','VL01');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values('104',N'Gương phẳng...',N'A.Gương và vật đối xứng nhau qua gương',N'B.Ảnh và vật cùng tính chất',
 N'C.Độ lớn vật, ảnh bằng nhau',N'D.Chỉ có câu B sai',N'Đáp án chính xác A','VL02');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values('105',N'Gen cấu trúc của vi khuẩn có đặc điểm gì?',N'A.Phân mảnh.',N'B.Vùng mã hóa không liên tục.',
 N'C.Không phân mảnh.',N'D. Không mã hóa axit amin mở đầu.',N'Đáp án chính xác C','SH01');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values('106',N'Tần số alen a của quần thể X đang là 0,5 qua vài thế hệ giảm bằng 0 nguyên nhân chính có lẽ là do:',
 N'A.Kích thước quần thể đã bị giảm mạnh', N'B.Môi trường thay đổi chống lại alen a',
 N'C.Đột biến gen A thành gen a',N'D.Có quá nhiều cá thể của quần thể đã di cư đi nơi khác.',N'Đáp án chính xác A ','SH02');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values('107',N'Nguyên thủ những nước nào sau đây tham dự Hội nghị Ianta (2/1945)?',N'A.Anh, Pháp, Mĩ.',N'B. Anh, Mĩ, Liên Xô.',
 N'C. Anh, Pháp, Đức.',N'D.Mĩ, Liên Xô, Trung Quốc.',N'Đáp án chính xác B','LS01');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values('108',N'Loại hình đồn điền nào phát triển mạnh ở Việt Nam trong thời kì 1919 - 1929?',N'A. Đồn điền trồng lúa.',N'B. Đồn điền trồng cao su.',
 N'C. Đồn điền trồng chè.',N'D. Đồn điền trồng cà phê.',N'Đáp án chính xác B','LS02');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values('109',N'Trong khu vực Đông Nam Á, dân số nước ta xếp thứ 3 sau',N'A. In-đô-nê-xi-a và Phi-lip-pin.',N'B. In-đô-nê-xi-a và Thái Lan.',
 N'C. In-đô-nê-xi-a và Mi-an-ma.', N'D. In-đô-nê-xi-a và Ma-lai-xi-a.',N'Đáp án chính xác A','DL01');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values('110',N'Xu hướng chuyển dịch của cơ cấu ngành kinh tế ở nước ta là :',N'A.Tăng tỉ trọng khu vực III, giảm tỉ trọng khu vực II.',
 N'B.Tăng tỉ trọng khu vực I, giảm tỉ trọng khu vực II.',
 N'C.Tăng tỉ trọng khu vực II, giảm ti trọng khu vực I.',N'D.Tăng ti trọng khu vực II, giảm tỉ trọng khu vực III.',N'Đáp án chính xác C','DL02');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values('111',N'Yếu tố nào tạo nên nội dung của pháp luật?',N'A.Các quy tắc xử sự chung.',N'B. Văn bản pháp luật.',N'C. Quy phạm pháp luật.',N'D. Cả A,B,C.',N'Đáp án chính các C','GDCD01');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values('112',N'Công dân không tuân thủ pháp luật khi tự ý thực hiện hành vi nào dưới đây?',N'A.Tố cáo công khai.',N'B. Khiếu nại tập thể.',
 N'C. Kinh doanh ngoại tệ.',N'D. Giải cứu con tin.',N'Đáp án chính xác C','GDCD02');

 --  update data cau hoi
 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('113',N'Trong các mệnh đề sau, mệnh đề nào sai?',N'A. Phép tịnh tiến biến đường thẳng thành đường thẳng song song hoặc trùng với nó.',
 N'B. Phép đối xứng trục biến đường thẳng thành đường thẳng song song hoặc trùng với nó.',
 N'C. Phép đối xứng tâm biến đường thẳng thành đường thẳng song song hoặc trùng với nó.',N'D. Phép vị tự biến đường thẳng thành đường thẳng song song hoặc trùng với nó.',N'Đáp án chính xác D','TO02');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('114',N'Trong mặt phẳng tọa độ Oxy, cho vectơ v =(2;-1) và điểm M( -3;2). Ảnh của điểm M qua phép tịnh tiến theo vectơ v là điểm có tọa độ nào trong các tọa độ sau?',
 N'A.5;3',N'B.1;1',N'C.-1;1',N'D.1;-1',N'Đáp án chính xác C','TO02');
 
insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('115',N'Trong mặt phẳng tọa độ Oxy, cho vectơ v =(1;-2) và điểm M( 3;1). Ảnh của điểm A qua phép tịnh tiến theo vectơ v là điểm B có tọa độ nào trong các tọa độ sau?',
 N'A. -2;-3',N'B. 2;3',N'C.4;-1',N'D.-1;4',N'Đáp án chính xác C','TO02');


 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('116',N'Chọn đáp án đúng nhất. Hàm số y = ax + b là hàm số bậc nhất khi',
 N'A.a = 0',N'B.a < 01',N'C.a > 0',N'D. a ≠ 0',N'Đáp án chính xác C','TO01');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('117',N'Chọn đáp án đúng nhất. Với a ≠ 0 hàm số y = ax + b là hàm số:',
 N'A. Bậc nhất', N'B. Hàm hằng', N'C.Đồng biến', N'D. Nghịch biến',N'Đáp án chính xác A','TO01');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('118',N'Số bào sau đây là căn bậc hai số học của số a = 0,36 :',N'A. – 0,6',N'B. 0,6 ',N'C. 0,9',N'D. – 0,18',N'Đáp án chính xác B','TO01');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('119',N'Phương trình 5x + 4y = 8 nhận cặp số nào sau đây làm nghiệm?', N'A.(-2;1)', N'B. (−1; 0)', N'C. (1,5; 3) ', N'D. (4; −3)',N'Đáp án chính xác D','TO01');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('120',N'Một người đứng trên đường quan sát chiếc ô tô chạy qua trước mặt. Dấu hiệu nào cho biết ô tô đang chuyển động?', N'A.Khói phụt ra từ ống thoát khí đặt dưới gầm xe.', N'B.Khoảng cách giữa xe và người đó thay đổi.', N'C.Bánh xe quay tròn.', N'D.Tiếng nổ của động cơ vang lên.',N'Đáp án chính xác B','VL01');

  insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('121',N'Trong các ví dụ dưới đây, trường hợp nào vật chuyển động được coi như là chất điểm?', N'A.Mặt Trăng quay quanh Trái Đất.', N'B.Đoàn tàu chuyển động trong sân ga.', N'  C. Em bé trượt từ đỉnh đến chân cầu trượt.', N'   D. Chuyển động tự quay của Trái Đất quanh trục.',N'Đáp án chính xác A','VL01');

  insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('122',N'Chọn đáp án đúng.', N'   A. Quỹ đạo là một đường thẳng mà trên đó chất điểm chuyển động.', N'B. Một đường cong mà trên đó chất điểm chuyển động gọi là quỹ đạo.', N' C. Quỹ đạo là một đường mà chất điểm vạch ra trong không gian khi nó chuyển động.', N'   D. Một đường vạch sẵn trong không gian trên đó chất điểm chuyển động gọi là quỹ đạo.',N'Đáp án chính xác B','VL01');

  insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('123',N'Khi chọn Trái Đất làm vật mốc thì câu nói nào sau đây đúng?', N'   A. Trái Đất quay quanh Mặt Trời.', N'B. Mặt Trời quay quanh Trái Đất.', N' C. Mặt Trời đứng yên còn Trái Đất chuyển động.', N' D. Cả Mặt Trời và Trái Đất đều chuyển động.',N'Đáp án chính xác B','VL01');

 
 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('124',N'Ánh sáng được phát ra dưới dạng gì? ',N' A. sóng điện từ', N'B. sóng âm', N' C. sóng cơ học', N' D. sóng áp suất',N'Đáp án chính xác A','VL02');

  insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('125',N'Điều gì xảy ra khi ánh sáng đi qua một khe nhỏ?',N'A.Ánh sáng bị phân tán',N' B. Ánh sáng bị tán xạ', N'C. Ánh sáng bị chiếu sáng', N'D. Ánh sáng bị giao thoa',N'Đáp án chính xác D','VL02');

   insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('126',N'Điều gì xảy ra khi ánh sáng đi qua một môi trường phẳng ?', N'Ánh sáng bị phản xạ',N'B. Ánh sáng bị tán xạ', N' C.Ánh sáng bị giảm tốc độ', N'D. Ánh sáng bị giao thoa',N'Đáp án chính xác A','VL02');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('127',N'Điều gì làm cho đối tượng trông đen khi ánh sáng chiếu vào nó?',N'A. Nó hấp thụ toàn bộ ánh sáng', N'B. Nó phản xạ toàn bộ ánh sáng',N'C. Nó tán xạ toàn bộ ánh sáng',N'D. Nó phản xạ một phần ánh sáng',N'Đáp án chính xác A','VL02');

 insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('128',N'Đâu là đặc điểm của di truyền tự do?',N'A.Có sự thay đổi gen của cá thể',N'B.Có sự chuyển gen giữa các loài',N'C. Không có sự can thiệp của con người',N'D. Có sự thay đổi môi trường sống',N'Đáp án chính xác C','SH01');

  insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('129',N'Điều gì xảy ra khi một gen bất thường được truyền từ cha hoặc mẹ sang cho con?',N'A.Con sẽ không bị ảnh hưởng',N'B.Con sẽ bị mắc bệnh',N'C.Con sẽ trở nên cao hơn',N'D.Con sẽ trở nên thông minh hơn',N'Đáp án chính xác B','SH01');

  insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('130',N'Đâu là cách thức di truyền của các loài sinh vật?',N'A.Phân tử protein', N'B.Phân tử RNA',N'C.Phân tử DNA',N'D. Phân tử lipid',N'Đáp án chính xác C','SH01');

  insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('131',N'Điều gì xảy ra khi một gen bất thường xuất hiện trong quá trình sinh sản?',N'A.Gen bất thường sẽ được loại bỏ',N'B.Gen bất thường sẽ được thay thế bằng gen bình thường',N'C. Gen bất thường sẽ được kế thừa',N'D.Gen bất thường không ảnh hưởng đến quá trình sinh sản',N'Đáp án chính xác C','SH01');

  insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('132',N'Theo lý thuyết Darwin, tiến hóa xảy ra như thế nào?',N'A.Tất cả các cá thể trong một loài đều giống nhau',N'B.Các cá thể có những đặc điểm phù hợp sẽ sống sót và chuyển gen cho thế hệ tiếp theo',N'C. Các cá thể có đặc điểm yếu sẽ sống sót và chuyển gen cho thế hệ tiếp theo',N'D.Tất cả các cá thể trong một loài đều có đặc điểm giống nhau',N'Đáp án chính xác B','SH02');

   insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('133',N'Đâu là ví dụ về tiến hóa hội tụ?',N'A.Chim cánh cụt và cá voi đều có vây bơi',N'B.Chim cánh cụt và chim bồ câu đều có lông',N'C.Chim cánh cụt và kiến trúc sư đều có mỏ',N'D.Chim cánh cụt và con người đều có khối lượng lớn',N'Đáp án chính xác A','SH02');

   insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('134',N'Điều gì làm cho một loài sinh vật có thể tiến hóa?',N'A.Sự can thiệp của con người',N'B.Sự chuyển đổi môi trường sống',N'C. Sự thay đổi gen của cá thể',N'D.Sự chuyển gen giữa các loài',N'Đáp án chính xác B','SH02');

   insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('135',N'Điều gì xảy ra khi hai loài sinh vật khác nhau chia sẻ một môi trường sống?',N'A.Họ sẽ cạnh tranh với nhau để giành tài nguyên',N'B.Họ sẽ tương tác với nhau để giúp cho cả hai tồn tại',N'C.Họ sẽ không ảnh hưởng lẫn nhau',N'D.Họ sẽ chuyển gen cho nhau để tạo ra một loài mới',N'Đáp án chính xác A','SH02');

 -- history 01
    insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('136',N'Chiến tranh thế giới thứ nhất bắt đầu vào năm nào?',N'A.1914',N'B.1918',N'C.1939',N'D.1945',N'Đáp án chính xác A','LS01');

    insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('137',N'Ai là người đứng đầu của Đức Quốc xã trong Thế chiến II?',N'A.Adolf Hitler',N'B.Benito Mussolini',N'C.Joseph Stalin',N'D.Winston Churchill',N'Đáp án chính xác A','LS01');

    insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('138',N'Thành phố nào bị tấn công bởi khủng bố vào ngày 11 tháng 9 năm 2001?',N'A.New York',N'B.Washington D.C.',N'C.Los Angeles',N'D.Chicago',N'Đáp án chính xác A','LS01');

    insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('139',N'Ai là người đứng đầu của Liên Xô trong Thế chiến II?',N'A. Adolf Hitler',N'B.Benito Mussolini',N'C.Joseph Stalin',N'D.Winston Churchill',N'Đáp án chính xác C','LS01');

 --history 02 
     insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('140',N'Đảng Cộng sản Việt Nam được thành lập vào năm nào?',N'A. 1930',N'B.1945',N'C.1954',N'D.1975',N'Đáp án chính xác A','LS02');

     insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('141',N'Đại hội đồng lần thứ nhất của Việt Nam Dân chủ Cộng hòa được tổ chức vào năm nào?',N'A.1946',N'B.1954',N'C.1960',N'D.1975',N'Đáp án chính xác A','LS02');

     insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('142',N'Cuộc Tết Mậu Thân xảy ra vào năm nào?',N'A. 1968',N'B.1972',N'C.1975',N'D.1980',N'Đáp án chính xác A','LS02');

     insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('143',N'Hiệp định Paris được ký kết vào năm nào, kết thúc cuộc chiến tranh Việt Nam?',N'A. 1965',N'B.1973',N'C.1975',N'D.1980',N'Đáp án chính xác B','LS02');

--geopraphy 001
     insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('144',N'Quốc gia có dân số đông nhất thế giới là?',N'A.Trung Quốc',N'B.Ân Độ ',N'C.Mỹ',N'D.Nga',N'Đáp án chính xác A','DL01');
      insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('145',N'Thành phố lớn nhất thế giới về diện tích là?',N'A.Tokyo, Nhật Bản',N'B.Mexico City, Mexico',N'C.New York City, Hoa Kỳ',N'D.Mumbai, Ấn Độ',N'Đáp án chính xác B','DL01');
     insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('146',N'Khu vực nào của thế giới có mật độ dân số cao nhất?',N'A.Châu Á',N'B.Châu Âu ',N'C. Châu Phi',N'D.Châu Mỹ',N'Đáp án chính xác A','DL01');
     insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('147',N'Quốc gia nào có mật độ dân số thấp nhất thế giới?',N'A.Canada',N'B.Úc ',N'C.Greenland',N'D.Nam Sudan',N'Đáp án chính xác C','DL01');
 --geopraphhy 002
      insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('148',N'Quốc gia nào là nền kinh tế lớn nhất thế giới hiện nay?',N'A.Mỹ',N'B.Trung Quốc ',N'C.Nhật Bản',N'D.Đức',N'Đáp án chính xác A','DL02');

      insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('149',N'Thành phố nào là trung tâm tài chính của châu Á?',N'A.Tokyo, Nhật Bản',N'B.Shanghai, Trung Quốc ',N'C.Hong Kong',N'D.Singapore',N'Đáp án chính xác C','DL02');

       insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('150',N'Ngành công nghiệp chủ lực của Thụy Điển là gì?',N'A.Công nghiệp điện tử',N'B.Công nghiệp ô tô',N'C.Công nghiệp giấy',N'D.Công nghiệp dầu khí',N'Đáp án chính xác D','DL02');

       insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('151',N'Quốc gia nào là nền kinh tế phát triển nhất châu Phi?',N'A.Nam Phi',N'B.Nigeria',N'C.Kenya',N'D.Ghana',N'Đáp án chính xác A','DL02');
 --GDCD 001

   insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('152',N'Tòa án cao nhất của Hoa Kỳ được gọi là gì?',N'A.Tòa án Tối cao',N'B.Tòa án Liên bang',N'C.Tòa án Quân sự',N'D.Tòa án Thượng thẩm',N'Đáp án chính xác A','GDCD01');

    insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('153',N'Luật sư nổi tiếng nào đã trở thành Tổng thống Hoa Kỳ?',N'A.John F. Kennedy',N'B.Barack Obama',N'C.Abraham Lincoln',N'D.Richard Nixon',N'Đáp án chính xác C','GDCD01');

     insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('154',N'Tội danh nào được gọi là tội phạm trắng án?',N'A.Tội giết người',N'B.Tội buôn lậu',N'C.Tội tham nhũng',N'D.Tội trốn thuế',N'Đáp án chính xác C','GDCD01');

     insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('155',N'Quyền lợi nào được bảo vệ bởi Hiến pháp Hoa Kỳ?',N'A. Quyền tự do ngôn luận',N'B.Quyền sở hữu vật chất',N'C.Quyền bầu cử',N'D.Tất cả các phương án đều đúng',N'Đáp án chính xác D','GDCD01');

 -- GDCD 002
      insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('156',N'Ai là người chịu trách nhiệm lớn nhất trong việc thực hiện pháp luật tại Hoa Kỳ?',N'A. Tổng thống',N'B.Quốc hội',N'C.Tòa án Tối cao',N'D.Luật sư tổng quát',N'Đáp án chính xác A','GDCD02');
  
  insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('157',N'Điều gì là cơ sở của hệ thống pháp luật Mỹ?',N'A.Hiến pháp Hoa Kỳ',N'B.Luật liên bang',N'C.TQuyết định tòa án',N'D.Tất cả các phương án đều đúng',N'Đáp án chính xác D','GDCD02');

       insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('158',N'Người bị cáo buộc trong một vụ án có quyền gì theo Hiến pháp Mỹ?',N'A.Quyền yên lặng',N'B.Quyền được tư vấn luật sư',N'C.Quyền được xét xử nhanh chóng và công bằng',N'D.Tất cả các phương án đều đúng',N'Đáp án chính xác D','GDCD02');

       insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('159',N'Tên chính thức của Hiến pháp Hoa Kỳ là gì?',N'A. Hiến pháp Liên bang của Hoa Kỳ',N'B.Hiến pháp Tối cao của Hoa Kỳ',N'C.Hiến pháp Quốc hội của Hoa Kỳ',N'D.Hiến pháp Tổng thống của Hoa Kỳ',N'Đáp án chính xác A','GDCD02');



insert into LOPHOC(malop, tenlop) values 
('71TO-VL', N'71K28 Lớp Toán - Vật Lý'),
('71SH-LS', N'71K28 Lớp Sinh - Sử'),
('71DL-CD', N'71K28 Lớp Địa - Công dân');
insert into LOP_MONHOC(malop, MaMonHoc) values 
('71TO-VL', '71TO'),
('71TO-VL', '71VL'),
('71SH-LS', '71SH'),
('71SH-LS', '71LS'),
('71DL-CD', '71DL'),
('71DL-CD', '71GDCD');


insert into chuongcacmonhoc_(MaChuong,TenChuong,MaMonHoc)
 values
 --GDCD
('GDCD03',N'Quyền bình đẳng giữa các dân tộc, tôn giáo',N'71GDCD'),
('GDCD04',N'Công dân với quyền tự do cơ bản',N'71GDCD'),
--DL
('DL03',N'Địa lý tự nhiên', '71DL'),
('DL04',N'Địa lý các vùng kinh tế', '71DL'),
--Lịch sử 
('LS03',N'Liên Xô và các nước Đông Âu (1945-1991).Liên Bang Nga (1991-2000)', '71LS'),
('LS04',N'Việt Nam từ năm 1945-1954', '71LS'),
--sinh học
('SH03',N'Sinh thái học', '71SH'),

--Vật lí
('VL03',N'Hạt nhân nguyên tử', '71VL'),
('VL04',N'Lượng tử ánh sáng', '71VL');

     insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
 values ('240',N'Đảng Cộng sản Việt Nam được thành lập vào năm nào?',N'A. 1930',N'B.1945',N'C.1954',N'D.1975',N'Đáp án chính xác A','LS03');

-- SQL Server syntax
--SELECT TOP 4 MaCauHoi
--FROM CacCauHoi
--where MaChuong = 'TO01' 
--ORDER BY RAND(CHECKSUM(NEWID()))

--SELECT COUNT (MaCauHoi) FROM cauhoimonthi_ where MaChuong = 'TO01';
--SELECT MaCauHoi FROM
--(SELECT MaCauHoi, ROW_NUMBER() OVER (ORDER BY MaCauHoi) AS RowNum FROM cauhoimonthi_ WHERE cauhoimonthi_.MaChuong = 'TO01') AS T
--WHERE RowNum = 1;


INSERT INTO DETHI(MADETHI, MaMonHoc) VALUES
('71TO_001','71TO');
INSERT INTO DETHI(MADETHI, MaMonHoc) VALUES
('71DL_001','71DL'),
('71VL_001','71VL'),
('71LS_001','71LS'),
('71SH_001','71SH'),
('71GDCD_001','71GDCD');






INSERT INTO DETHI_QLADMIN(MADETHI, TENDETHI, SOLUONGCAUHOI, TGLAMBAI, MALOP, MONTHI) VALUES
('71TO_001',N'Kiểm tra Toán Chương 1,2 lần thứ nhất', '8', '20', '71TO-VL', N'Toán học'),
('71DL_001',N'Kiểm tra Địa Lý lần thứ nhất', '8', '20', '71DL-CD', N'Địa lí'),
('71VL_001',N'Kiểm tra Vật Lí lần thứ nhất', '8', '20', '71TO-VL', N'Vật lý'),
('71LS_001',N'Kiểm tra Lịch Sử lần thứ nhất', '8', '20', '71SH-LS', N'Lịch sử'),
('71SH_001',N'Kiểm tra Sinh Học lần thứ nhất', '8', '20', '71SH-LS', N'Sinh học'),
('71GDCD_001',N'Kiểm tra Giáo Dục Công Dân lần thứ nhất', '8', '20', '71DL-CD', N'Giáo dục công dân');

--GDCD 03
insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('160',N'Các dân tộc đều được Nhà nước và pháp luật tôn trọng, tạo điều kiện phát triển mà không bị phân biệt đối xử là thể hiện quyền bình đẳng nào dưới đây ?',N'A. Bình đẳng giữa các dân tộc.',N'B. Bình đẳng giữa các địa phương.',N'C. Bình đẳng giữa các thành phần dân cư.',N'D. Bình đẳng giữa các tầng lớp xã hội.',N'Đáp án chính xác A','GDCD03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('161',N'Nội dung nào dưới đây nói về quyền bình đẳng giữa các dân tộc về văn hóa ?',N'A. Các dân tộc có nghĩa vụ phải sử dụng tiếng nói, chữ viết của mình.',N'B. Các dân tộc có quyền dùng tiếng nói, chữ viết của mình.',N'C. Các dân tộc có duy trì mọi phong tục, tập quán của dân tộc mình.',N'D. Các dân tộc không được duy trì những lê hộ riêng của dân tộc mình.',N'Đáp án chính xác B','GDCD03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('162',N'Một trong những nội dung về quyền bình đẳng giữa các dân tộc là ?',N'A. Truyền thống văn hóa tốt đẹp của các dân tộc đều được phát huy.',N'B. Dân tộc ít người không nên duy trì văn hóa của dân tộc mình.',N'C. Mọi phong tục, tập quán của các dân tộc đều cần được duy trì.',N'D. Chỉ duy trì văn hóa chung của dân tộc Việt Nam, không duy trì văn hóa riêng của mỗi dân tộc.',N'Đáp án chính xác A','GDCD03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('163',N'Các dân tộc có quyền dùng tiếng nói, chữ viết là thể hiện các dân tộc đều bình đẳng về lĩnh vực nào dưới đây ?',N'A. Kinh tế.',N'B. Chính trị.',N'C. Văn hóa, giáo dục.',N'D. Tự do tín ngưỡng.',N'Đáp án chính xác C','GDCD03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('164',N'Việc đảm bảo tỷ lệ thích hợp người dân tộc thiểu số trong các cơ quan quyền lực nhà nước là thể hiện',N'A. quyền bình đẳng giữa các dân tộc.','B. quyền bình đẳng giữa các công dân.',N'C. quyền bình đẳng giữa các vùng miền.',N'D. quyền bình đẳng trong công việc chung của Nhà nước.',N'Đáp án chính xác A','GDCD03');

--GDCD 04
insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('165',N'Tự ý bắt và giam giữ người không có căn cứ là hành vi xâm phạm tới quyền nào dưới đây của công dân ?',N'A. Quyền bất khả xâm phạm về thân thể.','B. Quyền được bảo hộ về tính mạng và sức khỏe.',N'C. Quyền tự do cá nhân.',N'D. Quyền tự do thân thể.',N'Đáp án chính xác A','GDCD04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('166',N'Người phạm tội quả tang hoặc đang bi truy nã thì',N'A. ai cũng có quyền bắt.',N'B. chỉ công an mới có quyền bắt.',N'C. phải xin lệnh khẩn cấp để bắt.',N'D. phải chờ ý kiến của cấp trên rồi mới được bắt.',N'Đáp án chính xác A','GDCD04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('167',N'Bắt người trong trưởng hợp khẩn cấp được tiến hành khi có căn cứ để cho rằng nguời đó',N'A. đang có ý dịnh phạm tội.',N'B. đang chuẩn bị thực hiện tội phạm rất nghiêm trọng.',N'C. đang lên kế hoạch thực hiện tội phạm.',N'D. đang họp bàn thực hiện tội phạm.',N'Đáp án chính xác B','GDCD04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('168',N'Hành vi nào dưới đây là xâm phạm đến sức khỏe của người khác ?',N'A. Đánh người gây thương tích.',N'B. Tự tiện bắt người.',N'C. Tự tiện giam giữ người.',N'D. Đe dọa đánh người.',N'Đáp án chính xác A','GDCD04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('169',N'Bắt người trong trường hợp nào dưới đây là đúng pháp luật ?',N'A. Khi có nghi ngờ người đó vừa mới thực hiện tội phạm..',N'B. Khi có nghi ngờ người đó đang chuẩn bị thực hiện tội phạm..',N'C. Khi có quyết định hoặc phê chuẩn của Viện kiểm sát.',N'D. Khi công can cần thu thập chứng cứ từ người đó.',N'Đáp án chính xác C','GDCD04');

-- địa lí 03
insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('170',N'Nước ta nằm ở vị trí:',N'A. Rìa phía Tây của bán đảo Đông Dương.',N'B. Rìa phía Đông của bán đảo Đông Dương.',N'C. Trung tâm châu Á.',N'D.Phía đông Đông Nam Á.',N'Đáp án chính xác B','DL03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('171',N'Nằm ở rìa phía Đông của bán đảo Đông Dương là nước:',N'A. Lào',N'B. Campuchia',N'C. Việt Nam',N'D. Mi-an-ma',N'Đáp án chính xác C','DL03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('172',N'Điểm cực Bắc của nước ta là xã Lũng Cú thuộc tỉnh:',N'A. Cao Bằng',N'B. Hà Giang',N'C. Yên Bái',N'D. Lạng Sơn',N'Đáp án chính xác B','DL03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('173',N'Vị trí địa lí của nước ta là:',N'A. nằm ở phía Đông bán đảo Đông Dương, gần trung tâm khu vực Đông Nam Á',N'B. nằm ở phía Tây bán đảo Đông Dương, gần trung tâm khu vực Đông Nam Á',N'C. nằm ở phía Đông bán đảo Đông Dương, gần trung tâm khu vực châu Á',N'D. nằm ở phía Tây bán đảo Đông Dương, gần trung tâm khu vực châu Á',N'Đáp án chính xác A','DL03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('174',N'Điểm cực Đông của nước ta là xã Vạn Thạnh thuộc tỉnh:',N'A. Ninh Thuận',N'B. Khánh Hòa',N'C. Đà Nẵng',N'D. Phú Yên',N'Đáp án chính xác B','DL03');

-- địa lí 04

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('175',N'Ý nào dưới đây không đúng với vùng Trung du và miền núi Bắc Bộ?',N'A. Có diện tích rộng nhất so với các vùng khác trong cả nước	',N'B. Có số dân đông nhất so với các vùng khác trong cả nước',N'C. Có sự phân hóa thành hai tiểu vùng',N'D. Tiếp giáp với Trung Quốc và Lào',N'Đáp án chính xác B','DL04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('176',N'Tỉnh nào dưới đây thuộc vùng TRung du và miền núi Bắc Bộ ?',N'A. Hà Nam  ',N'B. Thanh Hóa',N'C. Vĩnh Phúc ',N'D. Tuyên Quang',N'Đáp án chính xác D','DL04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('177',N'Tỉnh nào dưới đây vừa có cửa khẩu đường biển, vừa có cửa khẩu đường bộ với Trung Quốc?',N'A. Quảng Ninh ',N'B. Hà Giang',N'C. Hòa Bình ',N'D. Cao Bằng',N'Đáp án chính xác A','DL04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('178',N'Điều khác biệt về vị trí của vùng Trung du và miền núi Bắc Bộ so với các vùng khác trong cả nước là',N'A. Có biên giới kéo dài với Trung Quốc và Lào',N'B. Có tất cả các tỉnh giáp biển',N'C. Nằm ở vị trí trung chuyển giữa miền Bắc và miền Nam',N'D. Giáp Lào và Campuchia',N'Đáp án chính xác A','DL04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('179',N'So với khu vực Tây Bắc, khu vực Đông Bắc có',N'A. Mùa đông đến muộn và kết thúc sớm hơn ',N'B. Mùa đông đến muộn và kết thúc muộn hơn',N'C. Mùa đông đến sớm và kết thúc sớm hơn',N'D. Mùa đông đến sớm và kết thúc muộn hơn',N'Đáp án chính xác D','DL04');
-- lịch sử
insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('180',N'Những thành tựu Liên Xô đạt được trong công cuộc xây dựng chủ nghĩa xã hội tác động như thế nào đến tham vọng của Mĩ?',N'A. Tạo ra sự đối trọng với hệ thống tư bản chủ nghĩa',N'B. Làm đảo lộn chiến lược toàn cầu, khống chế thế giới của Mĩ',N'C. Tạo ra sự cân bằng về sức mạnh quân sự',N'D. Đưa quan hệ quốc tế trở lại trạng thái cân bằng',N'Đáp án chính xác B','LS03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('181',N'Sự chống phá của các thế lực thù địch có tác động như thế nào đến sự sụp đổ của CNXH ở Liên Xô và Đông Âu?',N'A. Là nguyên nhân sâu xa đưa đến sự sụp đổ',N'B. Là nguyên nhân quyết định sự sụp đổ',N'C. Là nguyên nhân khách quan dẫn đến sự sụp đổ',N'D. Không có tác động đến sự sụp đổ của Liên Xô',N'Đáp án chính xác C','LS03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('182',N'Vì sao việc thực hiện cơ chế tập trung quan liêu bao cấp lại là nguyên nhân sâu xa dẫn tới sự sụp đổ của chế độ XHCN ở Liên Xô và Đông Âu?',N'A. Thủ tiêu sự cạnh tranh, động lực phát triển, khiến đất nước trì trệ',N'B. Không phù hợp với một nền kinh tế phát triển theo chiều rộng',N'C. Tạo ra cái cớ để các thế lực thù địch chống phá',N'D. Không phù hợp với mô hình kinh tế XHCN',N'Đáp án chính xác A','LS03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('183',N'Nội dung nào sau đây phản ánh đúng nguyên nhân chủ yếu dẫn đến sự sụp đổ của CNXH ở Liên Xô và Đông Âu?',N'A. Chậm tiến hành cải tổ, khi cải tổ tiếp tục mắc phải sai lầm',N'B. Không bắt kịp sự phát triển của khoa học- kĩ thuật',N'C. Sự chống phá của các thế lực thù địch',N'D. Những hạn chế, thiếu sót trong bản thân nền kinh tế- xã hội tồn tại lâu dài',N'Đáp án chính xác A','LS03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('184',N'Nguyên nhân chủ yếu dẫn tới sự sụp đổ của CNXH ở Liên Xô và Đông Âu trong đầu thập niên 90 (thế kỉ XX)?',N'A.Khi cải cách lại mắc phải sai lầm ',N'B.Đường lối lãnh đạo mang tính chủ quan, duy ý chí',N'C. Sự chống phá của các thế lực thù địch',N'D. Không bắt kịp sự phát triển của khoa học - kĩ thuật',N'Đáp án chính xác B','LS03');


---lịch sử 04
insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('185',N'Sau Chiến tranh thế giới thứ hai, quân đội nước nào sau đây vào Việt Nam với danh nghĩa quân Đồng minh giải giáp phát xít Nhật?',N'A.Mỹ ',N'B.Pháp',N'C.Anh ',N'D.Liên Xô',N'Đáp án chính xác C','LS04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('186',N'Quân Trung Hoa Dân quốc khi tiến vào Việt Nam đã',N'A.sách nhiễu chính quyền cách mạng, đòi cải tổ Chính phủ, thay đổi quốc kì, Hồ Chí Minh phải từ chức. ',N'B.sử dụng một bộ phận Quân đội Nhật chờ giải giáp, đánh úp trụ sở chính quyền cách mạng.',N'C.ngầm giúp đỡ, trang bị vũ khí cho quân Pháp, ủng hộ các hành động khiêu khích quân sự của Pháp. ',N'D.cản trở về mặt ngoại giao, vận động các nước lớn không công nhận nước Việt Nam Dân chủ Cộng hoà.',N'Đáp án chính xác A','LS04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('187',N'Cuối tháng 8/1945, quân đội của các nước nào vào Việt Nam với danh nghĩa quân Đồng minh giải giáp phát xít?',N'A.Anh, Pháp. ',N'B.Anh, Trung Hoa Dân quốc.',N'C.Nhật, Pháp. ',N'D.Pháp, Trung Hoa Dân quốc.',N'Đáp án chính xác C','LS04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('188',N'Thuận lợi cơ bản của nước Việt Nam Dân chủ Cộng hoà sau Cách mạng tháng Tám năm 1945 là',N'A.ta đã nắm chính quyền trên tất cả các tỉnh thành trong cả nước. ',N'B.nhân dân đã giành được quyền làm chủ, rất gắn bó, ủng hộ chế độ mới.',N'C.nhận được ủng hộ nhiệt liệt của các nước trong phe xã hội chù nghĩa. ',N'D.hệ thống xã hội chủ nghĩa được hình thành trên thế giới.',N'Đáp án chính xác B','LS04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('189',N'Khó khăn lớn nhất mà nước Việt Nam Dân chủ Cộng hòa gặp phải ngay sau Cách mạng tháng Tám năm 1945 là',N'A.quân đội chưa được củng cố. ',N'B.nạn đói và nạn dốt.',N'C.nạn ngoại xâm và nội phản. ',N'D.ngân sách nhà nước trống rỗng.',N'Đáp án chính xác C','LS04');

--Vật ly 03
insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('190',N'Chọn phát biểu không đúng khi nói về hạt nhân nguyên từ:',N'A.Hai nguyên tử có điện tích hạt nhân khác nhau thuộc hai nguyên tố khác nhau.',N'B.Hai nguyên tử của hai nguyên tố bất kì khác nhau có số nơtron hoàn toàn khác nhau.',N'C.Hai nguyên tử có số nơtron khác nhau là hai đồng vị. ',N'D.Mọi hạt nhân của các nguyên tử đều có chứa cả proton và nơtron. ',N'Đáp án chính xác D','VL03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('191',N'Tính chất hóa học của một nguyên tố phụ thuộc vào:',N'A.khối lượng nguyên tử',N'B. điện tích của hạt nhân',N'C. bán kính hạt nhân ',N'D. năng lượng liên kết',N'Đáp án chính xác B','VL03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('192',N'Các nguyên tử được gọi là đồng vị khi các hạt nhân của chúng có:',N'A. số nuclôn giống nhau nhưng số nơtron khác nhau',N'B. số nơtron giống nhau nhưng số proton khác nhau',N'C. số proton giống nhau nhưng số nơtron khác nhau',N'D. khối lượng giống nhau nhưng số proton khác nhau',N'Đáp án chính xác C','VL03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('193',N'Các phản ứng hạt nhân không tuân theo định luật:',N'A. bảo toàn năng lượng',N'B. bảo toàn động lượng',N'C. bảo toàn động năng',N'D. bảo toàn số khối',N'Đáp án chính xác C','VL03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('194',N'Phát biểu nào dưới đây là sai khi nói về lực hạt nhân?',N'A. Có giá trị lớn hơn lực tương tác tĩnh điện giữa các proton.',N'B. Có tác dụng rất mạnh trong phạm vi hạt nhân.',N'C. Có thể là lực hút hoặc đẩy tùy theo khoảng cách giữa các nuclôn.',N'D. Không tác dụng khi các nuclôn cách xa nhau hơn kích thước hạt nhân.',N'Đáp án chính xác C','VL03');

--vật lý 04
insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('195',N'Tất cả cá phôtôn trong chân không có cùng:',N'A.Tốc độ',N'B. Bước sóng ',N'C.Năng lượng ',N'D.Tần số',N'Đáp án chính xác A','VL04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('196',N'Hiện tượng quang điện là hiện tượng êlectron bị bứt ra khỏi bề mặt tấm kim loại:',N'A. khi tấm kim loại bị nung nóng.',N'B. nhiễm điện do tiếp xúc với một vật nhiễm điện khác.',N'C. do bất kì nguyên nhân nào.',N'D. khi có ánh sáng thích hợp chiếu vào nó.',N'Đáp án chính xác D','VL04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('197',N'Công thoát êlectron của kim loại phụ thuộc vào:',N'A.bước sóng của ánh sáng kích thích và bản chất của kim loại',N'B.bản chất của kim loại',N'C.cường độ của chùm sáng kích thíchcường độ của chùm sáng kích thích',N'D.bước sóng của ánh sáng kích thích',N'Đáp án chính xác B','VL04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('198',N'Tìm phát biểu sai khi nói về thuyết lượng tử ánh sáng:',N'A.Nguyên tử hay phân tử vật chất không hấp thụ hay bức xạ ánh sáng một cách liên tục mà thành từng phần riêng biệt, đứt quãng.',N'B.Ánh sáng được tạo bởi các hạt gọi là phôtôn.',N'C.Năng lượng của các phôtôn ánh sáng là như nhau, không phụ thuộc vào bước sóng ánh sáng.',N'D.Khi ánh sáng truyền đi, các lượng tử ánh sáng không thay đổi và không phụ thuộc vào khoảng cách tới nguồn sáng.',N'Đáp án chính xác C','VL04');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('199',N'Trong chân không, bức xạ đơn sắc màu vàng có bước sóng 0,589 µm. Năng lượng của phôtôn ứng với bức xạ này là:',N'A.0,21 eV.',N'B.2,11 eV.',N'C.4,22 eV',N'D.0,42 eV.',N'Đáp án chính xác B','VL04');

-- sinh học 03 
insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('200',N'Môi trường sống của sinh vật gồm có:',N'A.Đất-nước-không khí',N'B.Đất-nước-không khí-sinh vật',N'C.Đất-nước-không khí-trên cạn',N'D.Đất-nước-trên cạn-sinh vật',N'Đáp án chính xác D','SH03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('201',N'Sự khác nhau chủ yếu giữa môi trường nước và môi trường cạn là',N'A.Nước có nhiều khoáng hơn đất.',N'B.Cường độ ánh sáng ở môi trường cạn cao hơn môi trường nước.',N'C.Nồng độ ôxi ở môi trường cạn cao hơn ở môi trường nước.Nồng độ ôxi ở môi trường cạn cao hơn ở môi trường nước.',N'D.Nước có độ nhớt thấp hơn không khí.',N'Đáp án chính xác C','SH03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('202',N' Điểm khác nhau giữa môi trường nước và môi trường cạn nào sau đây là đúng?',N'A.Khoáng chất ở trên cạn nhiều hơn dưới nước.',N'B.Ánh sáng dưới nước nhiều hơn ở trên cạn.',N'C.Nhiệt độ trên cạn luôn cao hơn dưới nước.',N'D.Nồng độ oxy dưới nước thấp hơn trên cạn.',N'Đáp án chính xác D','SH03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('203',N'Loài vi khuẩn Rhizobium sống cộng sinh với cây họ Đậu để đảm bảo cung cấp môi trường kị khí cho việc cố định nito, chúng có môi trường sống là',N'A.Trên cạn',N'B.Sinh vật',N'C.Đất',N'D.Nước',N'Đáp án chính xác B','SH03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('204',N'Các loài thực vật thủy sinh có môi trường sống là',N'A.Trên cạn',N'B.Sinh vật',N'C.Đất',N'D.Nước',N'Đáp án chính xác D','SH03');

insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong)
values('205',N'Các nhân tố sinh thái được chia thành hai nhóm sau:',N'A.Nhóm nhân tố sinh thái sinh vật và con người.',N'B.Nhóm nhân tố sinh thái vô sinh và hữu sinh',N'C.Nhóm nhân tố sinh thái trên cạn và dưới nước.Nhóm nhân tố sinh thái trên cạn và dưới nước.',N'D.Nhóm nhân tố sinh thái bất lợi và có lợi.',N'Đáp án chính xác B','SH03');

insert into HOCVIEN(MALOP, MAHV, HOLOT, TEN, PHAI, SODT) 
values
('71TO-VL', 'HV01', N'Nguyễn Văn', 	N'Anh', 'Nam',	'0987654321'),
('71TO-VL', 'HV02', N'Trần Thị', 	N'Lan', N'Nữ',	'0123456789'),
('71TO-VL', 'HV03', N'Lê Văn', 	N'Nam', 'Nam',	'0987654321'),
('71SH-LS', 'HV04', N'Phạm Thị', 	N'Mai', N'Nữ',	'0123456789'),
('71SH-LS', 'HV05', N'Nguyễn Thanh', 	N'Huy', 'Nam',	'0987654321'),
('71SH-LS', 'HV06', N'Đỗ Thị', 	N'Hằng', N'Nữ',	'0123456789'),
('71DL-CD', 'HV07', N'Lý Văn', 	N'Minh', 'Nam',	'0987654321'),
('71DL-CD', 'HV08', N'Hoàng Thị', 	N'Nga', N'Nữ',	'0123456789'),
('71DL-CD', 'HV09', N'Trịnh Văn', 	N'Quân', 'Nam',	'0987654321');

INSERT INTO HOCVIEN (MALOP, MAHV, HOLOT, TEN, PHAI, SODT)
VALUES
('71DL-CD', 'HV10', 'Nguyễn Văn', 'An', 'Nam', '0123456789'),
('71TO-VL', 'HV11', 'Trần Thị', 'Bình', N'Nữ', '0987654321'),
('71SH-LS', 'HV12', 'Lê Minh', 'Châu', 'Nam', '0912345678'),
('71DL-CD','HV13', 'Phạm Thị', 'Dung', N'Nữ', '0909090909'),
('71TO-VL','HV14', 'Đỗ Quang', 'Hải', 'Nam', '0939393939'),
('71SH-LS','HV15', 'Hoàng Thị', 'Hoa', N'Nữ', '0969696969'),
('71TO-VL', 'HV16', 'Nguyễn', 'Anh', 'Nam', '0123456789');

INSERT INTO CHITIET_DETHI(MADETHI, MACAUHOI) VALUES
('71TO_001','118'),
('71TO_001','119'),
('71TO_001','113'),
('71TO_001','101'),
('71TO_001','102'),
('71TO_001','116'),
('71TO_001','117'),
('71TO_001','115');
--
INSERT INTO CHITIET_DETHI(MADETHI, MACAUHOI) VALUES
('71DL_001','109'),
('71DL_001','144'),
('71DL_001','145'),
('71DL_001','148'),
('71DL_001','149'),
('71DL_001','151'),
('71DL_001','170'),
('71DL_001','171');

INSERT INTO CHITIET_DETHI(MADETHI, MACAUHOI) VALUES
('71VL_001','103'),
('71VL_001','120'),
('71VL_001','121'),
('71VL_001','125'),
('71VL_001','190'),
('71VL_001','193'),
('71VL_001','197'),
('71VL_001','199');

INSERT INTO CHITIET_DETHI(MADETHI, MACAUHOI) VALUES
('71LS_001','108'),
('71LS_001','136'),
('71LS_001','139'),
('71LS_001','140'),
('71LS_001','141'),
('71LS_001','142'),
('71LS_001','180'),
('71LS_001','181');

INSERT INTO CHITIET_DETHI(MADETHI, MACAUHOI) VALUES
('71SH_001','105'),
('71SH_001','128'),
('71SH_001','130'),
('71SH_001','132'),
('71SH_001','134'),
('71SH_001','201'),
('71SH_001','203'),
('71SH_001','205');

INSERT INTO CHITIET_DETHI(MADETHI, MACAUHOI) VALUES
('71GDCD_001','111'),
('71GDCD_001','112'),
('71GDCD_001','153'),
('71GDCD_001','156'),
('71GDCD_001','157'),
('71GDCD_001','164'),
('71GDCD_001','166'),
('71GDCD_001','169');

