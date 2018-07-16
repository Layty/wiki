/**
 * ��һ���ַ�ת�������
 */
static int getIntFromChar(char c) {
    int result = (int) c;
    return result & 0x000000ff;
}

/**
 * ��16���ַ�ת���4X4�����飬
 * �þ������ֽڵ�����˳��Ϊ���ϵ��£�
 * �������������С�
 */
static void convertToIntArray(char *str, int pa[4][4]) {
    int k = 0;
    int i,j;
    for(i = 0; i < 4; i++)
        for(j = 0; j < 4; j++) {
            pa[j][i] = getIntFromChar(str[k]);
            k++;
        }
}

/**
 * ��������4���ַ��ϲ���һ��4�ֽڵ�����
 */
static int getWordFromStr(char *str) {
    int one, two, three, four;
    one = getIntFromChar(str[0]);
    one = one << 24;
    two = getIntFromChar(str[1]);
    two = two << 16;
    three = getIntFromChar(str[2]);
    three = three << 8;
    four = getIntFromChar(str[3]);
    return one | two | three | four;
}

/**
 * ��һ��4�ֽڵ����ĵ�һ�����������ĸ��ֽ�ȡ����
 * ���һ��4��Ԫ�ص������������档
 */
static void splitIntToArray(int num, int array[4]) {
    int one, two, three;
    one = num >> 24;
    array[0] = one & 0x000000ff;
    two = num >> 16;
    array[1] = two & 0x000000ff;
    three = num >> 8;
    array[2] = three & 0x000000ff;
    array[3] = num & 0x000000ff;
}


/**
 * �������е�Ԫ��ѭ������stepλ
 */
static void leftLoop4int(int array[4], int step) {
    int temp[4];
    for(int i = 0; i < 4; i++)
        temp[i] = array[i];

    int index = step % 4 == 0 ? 0 : step % 4;
    for(int i = 0; i < 4; i++){
        array[i] = temp[index];
        index++;
        index = index % 4;
    }
}


/**
 * �������еĵ�һ������������Ԫ�طֱ���Ϊ
 * 4�ֽ����͵ĵ�һ�������������ֽڣ��ϲ���һ��4�ֽ�����
 */
static int mergeArrayToInt(int array[4]) {
    int one = array[0] << 24;
    int two = array[1] << 16;
    int three = array[2] << 8;
    int four = array[3];
    return one | two | three | four;
}


/*********************************************************************************************
��Կ��չ
*/
//��Կ��Ӧ����չ����
static int w[44];

/**
 * ��չ��Կ������ǰ�w[44]�е�ÿ��Ԫ�س�ʼ��
 */
static void extendKey(char *key) {
    for(int i = 0; i < 4; i++)
        w[i] = getWordFromStr(key + i * 4); 

    for(int i = 4, j = 0; i < 44; i++) {
        if( i % 4 == 0) {
            w[i] = w[i - 4] ^ T(w[i - 1], j); 
            j++;//��һ��
        }else {
            w[i] = w[i - 4] ^ w[i - 1]; 
        }
    }   

}


/**
 * ������ֵ��
 */
static const int Rcon[10] = { 0x01000000, 0x02000000,
    0x04000000, 0x08000000,
    0x10000000, 0x20000000,
    0x40000000, 0x80000000,
    0x1b000000, 0x36000000 };
/**
 * ��Կ��չ�е�T����
 */
static int T(int num, int round) {
    int numArray[4];
    splitIntToArray(num, numArray);//��int��num���� ��ȡ��u8���ֽ�������ȥ
    leftLoop4int(numArray, 1);//��ѭ��

    //�ֽڴ���
    for(int i = 0; i < 4; i++)
        numArray[i] = getNumFromSBox(numArray[i]);

    int result = mergeArrayToInt(numArray);//���ֽ�����ת��Ϊint����
    return result ^ Rcon[round];
}







/**
 * ��ӡ4X4������
 */
static void printArray(int a[4][4]) {
    int i,j;
    for(i = 0; i < 4; i++){
        for(j = 0; j < 4; j++)
            printf("a[%d][%d] = 0x%x ", i, j, a[i][j]);
        printf("\n");
    }
    printf("\n");
}

/**
 * ��ӡ�ַ�����ASSCI��
 * ��ʮ��������ʾ��
 */
static void printASSCI(char *str, int len) {
    int i;
    for(i = 0; i < len; i++)
        printf("0x%x ", getIntFromChar(str[i]));
    printf("\n");
}
